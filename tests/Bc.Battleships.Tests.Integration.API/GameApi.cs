using AwesomeAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.API;

/// <summary>HTTP helpers shared by the API integration tests.</summary>
internal static class GameApi
{
    public const string ProblemContentType = "application/problem+json";
    public const int Unknown = 0;
    public const int Hit = 2;

    public static CancellationToken Ct => TestContext.Current.CancellationToken;

    public static async Task<Guid> CreateGame(HttpClient client)
    {
        var response = await client.PostAsync("/games", null, Ct);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        return (await ReadJson(response)).GetProperty("id").GetGuid();
    }

    public static async Task<JsonElement> GetGameJson(HttpClient client, Guid gameId)
    {
        var response = await client.GetAsync($"/games/{gameId}", Ct);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        return await ReadJson(response);
    }

    public static Task<HttpResponseMessage> Shoot(HttpClient client, Guid gameId, int column, int row)
        => client.PostAsJsonAsync($"/games/{gameId}/shots", new { column, row }, Ct);

    public static int FieldAt(JsonElement game, int column, int row)
        => game.GetProperty("board").GetProperty("boardRows")[row].GetProperty("row")[column].GetInt32();

    public static async Task<JsonElement> ReadJson(HttpResponseMessage response)
        => await response.Content.ReadFromJsonAsync<JsonElement>(Ct);
}
