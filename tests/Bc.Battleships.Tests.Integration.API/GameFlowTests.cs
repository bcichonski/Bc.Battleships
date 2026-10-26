using AwesomeAssertions;
using Bc.Battleships.Tests.Common;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using static Bc.Battleships.Tests.Integration.API.GameApi;

namespace Bc.Battleships.Tests.Integration.API;

public class GameFlowTests : IDisposable
{
    private const int BoardSize = 4;
    private const int TotalHitPoints = 3;

    private readonly BattleshipsWebApplicationFactory _factory = new();
    // Two ships (lengths 2 and 1) on a 4x4 board: placement is random, so assertions only rely on the hit count.
    private readonly HttpClient _client;

    public GameFlowTests()
    {
        _client = _factory.CreateClient(BoardSize, [2, 1]);
    }

    [Fact]
    public async Task NewGame_ShootEveryCell_SinksShipsAndEndsTheGame()
    {
        var gameId = await CreateGame(_client);
        var created = await GetGameJson(_client, gameId);
        created.GetProperty("isGameActive").GetBoolean().Should().BeTrue();
        created.GetProperty("currentHitPoints").GetInt32().Should().Be(TotalHitPoints);

        var hitsSoFar = 0;
        var activeAfterFirstHit = false;
        foreach (var (column, row) in AllCells())
        {
            var response = await Shoot(_client, gameId, column, row);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var shot = await ReadJson(response);
            var hit = shot.GetProperty("hit").GetBoolean();
            if (hit)
            {
                hitsSoFar++;
            }

            shot.GetProperty("currentHitPoints").GetInt32().Should().Be(TotalHitPoints - hitsSoFar);
            var game = await GetGameJson(_client, gameId);
            game.GetProperty("currentHitPoints").GetInt32().Should().Be(TotalHitPoints - hitsSoFar);
            game.GetProperty("isGameActive").GetBoolean().Should().Be(hitsSoFar < TotalHitPoints);
            if (hit && hitsSoFar == 1)
            {
                activeAfterFirstHit = game.GetProperty("isGameActive").GetBoolean();
            }

            if (hitsSoFar == TotalHitPoints)
            {
                break; // the winning hit; the game accepts no further shots
            }
        }

        hitsSoFar.Should().Be(TotalHitPoints);
        activeAfterFirstHit.Should().BeTrue();

        var finished = await GetGameJson(_client, gameId);
        finished.GetProperty("isGameActive").GetBoolean().Should().BeFalse();
        finished.GetProperty("currentHitPoints").GetInt32().Should().Be(0);
        AllCells().Count(cell => FieldAt(finished, cell.Column, cell.Row) == Hit).Should().Be(TotalHitPoints);

        var rejected = await Shoot(_client, gameId, column: 0, row: 0);
        rejected.StatusCode.Should().Be(HttpStatusCode.Conflict);
        rejected.Content.Headers.ContentType!.MediaType.Should().Be(ProblemContentType);
        (await GetGameJson(_client, gameId)).GetRawText().Should().Be(finished.GetRawText());
    }

    [Fact]
    public async Task ShootingAHitCellAgain_ReportsTheSameHitAndKeepsHitPoints()
    {
        var gameId = await CreateGame(_client);
        var (column, row) = await FindCell(gameId, hit: true);
        var hitPoints = (await GetGameJson(_client, gameId)).GetProperty("currentHitPoints").GetInt32();

        var again = await ReadJson(await Shoot(_client, gameId, column, row));

        again.GetProperty("hit").GetBoolean().Should().BeTrue();
        again.GetProperty("currentHitPoints").GetInt32().Should().Be(hitPoints);
        (await GetGameJson(_client, gameId)).GetProperty("currentHitPoints").GetInt32().Should().Be(hitPoints);
    }

    [Fact]
    public async Task ShootingWaterAgain_ReportsTheSameMissAndKeepsHitPoints()
    {
        var gameId = await CreateGame(_client);
        var (column, row) = await FindCell(gameId, hit: false);
        var hitPoints = (await GetGameJson(_client, gameId)).GetProperty("currentHitPoints").GetInt32();

        var again = await ReadJson(await Shoot(_client, gameId, column, row));

        again.GetProperty("hit").GetBoolean().Should().BeFalse();
        again.GetProperty("currentHitPoints").GetInt32().Should().Be(hitPoints);
        (await GetGameJson(_client, gameId)).GetProperty("currentHitPoints").GetInt32().Should().Be(hitPoints);
    }

    [Fact]
    public async Task ShotOnFinishedGame_IsRejectedAndNotRecorded()
    {
        using var client = _factory.CreateClient(boardSize: 1, shipLengths: [1]);
        var gameId = await CreateGame(client);
        await Shoot(client, gameId, column: 0, row: 0);
        var finished = await GetGameJson(client, gameId);

        var response = await Shoot(client, gameId, column: 0, row: 0);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        (await GetGameJson(client, gameId)).GetRawText().Should().Be(finished.GetRawText());
    }

    // Shoots cells in order until one reports the wanted result. The first hit cannot win the game (3 hit points)
    // and 13 of the 16 cells are water, so both searches always succeed on an active game.
    private async Task<(int Column, int Row)> FindCell(Guid gameId, bool hit)
    {
        foreach (var (column, row) in AllCells())
        {
            var shot = await ReadJson(await Shoot(_client, gameId, column, row));
            if (shot.GetProperty("hit").GetBoolean() == hit)
            {
                return (column, row);
            }
        }

        throw new InvalidOperationException($"No cell with hit={hit} found.");
    }

    private static (int Column, int Row)[] AllCells()
        => Enumerable.Range(0, BoardSize)
            .SelectMany(row => Enumerable.Range(0, BoardSize).Select(column => (column, row)))
            .ToArray();

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
        GC.SuppressFinalize(this);
    }
}
