using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Bc.Battleships.Tests.Common;
using Xunit;
using static Bc.Battleships.Tests.Integration.API.GameApi;

namespace Bc.Battleships.Tests.Integration.API;

public class GameEndpointsTests : IDisposable
{
    private readonly BattleshipsWebApplicationFactory _factory = new();
    private readonly HttpClient _client;

    public GameEndpointsTests()
    {
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task PostGames_Returns201WithLocationAndGameWithIdAndBoard()
    {
        var response = await _client.PostAsync("/games", null, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var json = await ReadJson(response);
        var id = json.GetProperty("id").GetGuid();
        id.Should().NotBeEmpty();
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.OriginalString.Should().Be($"/games/{id}");
        json.GetProperty("isGameActive").GetBoolean().Should().BeTrue();
        json.GetProperty("currentHitPoints").GetInt32().Should().Be(13);

        var board = json.GetProperty("board");
        var size = board.GetProperty("boardSize").GetInt32();
        size.Should().Be(10);
        var rows = board.GetProperty("boardRows");
        rows.GetArrayLength().Should().Be(size);
        foreach (var row in rows.EnumerateArray())
        {
            var fields = row.GetProperty("row");
            fields.GetArrayLength().Should().Be(size);
            foreach (var field in fields.EnumerateArray())
            {
                field.ValueKind.Should().Be(JsonValueKind.Number);
                field.GetInt32().Should().Be(Unknown);
            }
        }
    }

    [Fact]
    public async Task PostGames_DoesNotChangeAnotherGame()
    {
        var first = await CreateGame(_client);
        await Shoot(_client, first, column: 0, row: 0);
        var firstBefore = await GetGameJson(_client, first);

        await CreateGame(_client);

        var firstAfter = await GetGameJson(_client, first);
        firstAfter.GetRawText().Should().Be(firstBefore.GetRawText());
    }

    [Fact]
    public async Task GetGame_ById_ReturnsTheCreatedGame()
    {
        var createResponse = await _client.PostAsync("/games", null, Ct);
        var created = await ReadJson(createResponse);

        var response = await _client.GetAsync(createResponse.Headers.Location, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await ReadJson(response)).GetRawText().Should().Be(created.GetRawText());
    }

    [Fact]
    public async Task GetGame_WithUnknownId_Returns404ProblemDetails()
    {
        var response = await _client.GetAsync($"/games/{Guid.NewGuid()}", Ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType!.MediaType.Should().Be(ProblemContentType);
        (await ReadJson(response)).GetProperty("status").GetInt32().Should().Be(404);
    }

    [Fact]
    public async Task GetGame_WithMalformedId_Returns404()
    {
        var response = await _client.GetAsync("/games/not-a-guid", Ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostShot_WithUnknownGame_Returns404ProblemDetails()
    {
        var response = await _client.PostAsJsonAsync($"/games/{Guid.NewGuid()}/shots", new { column = 0, row = 0 }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType!.MediaType.Should().Be(ProblemContentType);
    }

    [Fact]
    public async Task PostShot_WithMalformedGameId_Returns404()
    {
        var response = await _client.PostAsJsonAsync("/games/not-a-guid/shots", new { column = 0, row = 0 }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostShot_ChangesOnlyTheTargetedGame()
    {
        var targeted = await CreateGame(_client);
        var other = await CreateGame(_client);
        var otherBefore = await GetGameJson(_client, other);

        var response = await Shoot(_client, targeted, column: 3, row: 2);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        FieldAt(await GetGameJson(_client, targeted), column: 3, row: 2).Should().NotBe(Unknown);
        (await GetGameJson(_client, other)).GetRawText().Should().Be(otherBefore.GetRawText());
    }

    [Fact]
    public async Task PostShot_WithMiss_ReturnsHitFalseAndUnchangedHitPoints()
    {
        // A 2x2 board with one ship of length 2: the ship fills one row or one column, so exactly one of the
        // diagonal cells is water and at most one hit lands before the miss (the game cannot be won first).
        using var client = CreateClient(boardSize: 2, shipLengths: [2]);
        var gameId = await CreateGame(client);

        var miss = await FindShot(client, gameId, DiagonalOf(2), hit: false);

        miss.GetProperty("hit").GetBoolean().Should().BeFalse();
        miss.GetProperty("currentHitPoints").GetInt32()
            .Should().Be((await GetGameJson(client, gameId)).GetProperty("currentHitPoints").GetInt32());
    }

    [Fact]
    public async Task PostShot_WithHit_DecreasesHitPoints()
    {
        // Same layout as the miss test: one of the two diagonal cells is always part of the ship.
        using var client = CreateClient(boardSize: 2, shipLengths: [2]);
        var gameId = await CreateGame(client);

        var hit = await FindShot(client, gameId, DiagonalOf(2), hit: true);

        hit.GetProperty("hit").GetBoolean().Should().BeTrue();
        hit.GetProperty("currentHitPoints").GetInt32().Should().Be(1);
    }

    [Fact]
    public async Task PostShot_AfterGameIsWon_Returns409AndKeepsTheGameUnchanged()
    {
        using var client = CreateClient(boardSize: 1, shipLengths: [1]);
        var gameId = await CreateGame(client);
        var winningShot = await ReadJson(await Shoot(client, gameId, column: 0, row: 0));
        winningShot.GetProperty("hit").GetBoolean().Should().BeTrue();
        winningShot.GetProperty("currentHitPoints").GetInt32().Should().Be(0);
        var wonGame = await GetGameJson(client, gameId);

        var response = await Shoot(client, gameId, column: 0, row: 0);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        response.Content.Headers.ContentType!.MediaType.Should().Be(ProblemContentType);
        var afterRejectedShot = await GetGameJson(client, gameId);
        afterRejectedShot.GetProperty("currentHitPoints").GetInt32().Should().Be(0);
        afterRejectedShot.GetRawText().Should().Be(wonGame.GetRawText());
    }

    [Fact]
    public async Task GetGame_AfterGameIsWon_ReturnsInactiveGameWithBoard()
    {
        using var client = CreateClient(boardSize: 1, shipLengths: [1]);
        var gameId = await CreateGame(client);
        await Shoot(client, gameId, column: 0, row: 0);

        var json = await GetGameJson(client, gameId);

        json.GetProperty("id").GetGuid().Should().Be(gameId);
        json.GetProperty("isGameActive").GetBoolean().Should().BeFalse();
        json.GetProperty("currentHitPoints").GetInt32().Should().Be(0);
        FieldAt(json, column: 0, row: 0).Should().Be(Hit);
    }

    [Fact]
    public async Task GetGame_AfterReload_ShowsHitAtExactlyTheShotCell()
    {
        // One ship of length 5 fills a row or column of a 5x5 board, so exactly one diagonal cell is a hit
        // (the game cannot be won). Each request reloads the game from SQLite.
        using var client = CreateClient(boardSize: 5, shipLengths: [5]);
        var gameId = await CreateGame(client);

        var hitCell = (Column: -1, Row: -1);
        foreach (var (column, row) in DiagonalOf(5))
        {
            var shot = await ReadJson(await Shoot(client, gameId, column, row));
            if (shot.GetProperty("hit").GetBoolean())
            {
                hitCell = (column, row);
            }
        }

        hitCell.Column.Should().BeGreaterThanOrEqualTo(0);
        var json = await GetGameJson(client, gameId);
        var hits = Enumerable.Range(0, 5)
            .SelectMany(row => Enumerable.Range(0, 5).Select(column => (Column: column, Row: row)))
            .Where(cell => FieldAt(json, cell.Column, cell.Row) == Hit)
            .ToList();
        hits.Should().Equal(hitCell);
    }

    [Fact]
    public async Task PostGames_WithBoardSizeConfigured_ReturnsBoardOfThatSize()
    {
        using var client = CreateClient(boardSize: 6, shipLengths: [3, 2]);

        var json = await GetGameJson(client, await CreateGame(client));

        json.GetProperty("board").GetProperty("boardSize").GetInt32().Should().Be(6);
        json.GetProperty("currentHitPoints").GetInt32().Should().Be(5);
    }

    [Fact]
    public async Task OpenApiDocument_ListsTheGameOperations()
    {
        var json = await ReadJson(await _client.GetAsync("/openapi/v1.json", Ct));

        var paths = json.GetProperty("paths");
        var games = paths.GetProperty("/games");
        games.GetProperty("post").GetProperty("responses").TryGetProperty("201", out _).Should().BeTrue();

        var game = paths.GetProperty("/games/{id}").GetProperty("get").GetProperty("responses");
        game.TryGetProperty("200", out _).Should().BeTrue();
        game.TryGetProperty("404", out _).Should().BeTrue();

        var shots = paths.GetProperty("/games/{id}/shots").GetProperty("post").GetProperty("responses");
        shots.TryGetProperty("200", out _).Should().BeTrue();
        shots.TryGetProperty("404", out _).Should().BeTrue();
        shots.TryGetProperty("409", out _).Should().BeTrue();
    }

    private HttpClient CreateClient(int boardSize, int[] shipLengths)
        => _factory.CreateClient(boardSize, shipLengths);

    private static IEnumerable<(int Column, int Row)> DiagonalOf(int boardSize)
        => Enumerable.Range(0, boardSize).Select(i => (i, i));

    private static async Task<JsonElement> FindShot(HttpClient client, Guid gameId, IEnumerable<(int Column, int Row)> cells, bool hit)
    {
        foreach (var (column, row) in cells)
        {
            var response = await Shoot(client, gameId, column, row);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = await ReadJson(response);
            if (json.GetProperty("hit").GetBoolean() == hit)
            {
                return json;
            }
        }

        throw new InvalidOperationException($"No shot with hit={hit} found on the searched cells.");
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
        GC.SuppressFinalize(this);
    }
}
