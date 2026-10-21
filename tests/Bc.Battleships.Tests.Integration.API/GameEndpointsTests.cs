using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AwesomeAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Bc.Battleships.Tests.Integration.API
{
    public class GameEndpointsTests : IDisposable
    {
        private readonly string _databasePath = Path.Combine(Path.GetTempPath(), $"battleships.endpoints.{Path.GetRandomFileName()}.db");
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public GameEndpointsTests()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
                builder.ConfigureAppConfiguration((_, configuration) =>
                    configuration.AddInMemoryCollection(new[]
                    {
                        new System.Collections.Generic.KeyValuePair<string, string?>(
                            "ConnectionStrings:ConnectionString", $"Data Source={_databasePath}")
                    })));
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_WithNoGame_ReturnsInactiveGameWithNullBoard()
        {
            var response = await _client.GetAsync("/game", Ct);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = await ReadJson(response);
            json.GetProperty("isGameActive").GetBoolean().Should().BeFalse();
            json.GetProperty("currentHitPoints").GetInt32().Should().Be(0);
            json.GetProperty("board").ValueKind.Should().Be(JsonValueKind.Null);
        }

        [Fact]
        public async Task Post_ReturnsOkWithEmptyBody()
        {
            var response = await _client.PostAsync("/game", new StringContent(string.Empty), Ct);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync(Ct)).Should().BeEmpty();
        }

        [Fact]
        public async Task Get_AfterPost_ReturnsActiveGameWithNumericBoard()
        {
            await _client.PostAsync("/game", new StringContent(string.Empty), Ct);

            var response = await _client.GetAsync("/game", Ct);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = await ReadJson(response);
            json.GetProperty("isGameActive").GetBoolean().Should().BeTrue();
            json.GetProperty("currentHitPoints").GetInt32().Should().BeGreaterThan(0);
            var board = json.GetProperty("board");
            var size = board.GetProperty("boardSize").GetInt32();
            size.Should().BeGreaterThan(0);
            var rows = board.GetProperty("boardRows");
            rows.GetArrayLength().Should().Be(size);
            foreach (var row in rows.EnumerateArray())
            {
                var fields = row.GetProperty("row");
                fields.GetArrayLength().Should().Be(size);
                foreach (var field in fields.EnumerateArray())
                {
                    field.ValueKind.Should().Be(JsonValueKind.Number);
                }
            }
        }

        [Fact]
        public async Task Put_WithMiss_ReturnsHitFalseAndUnchangedHitPoints()
        {
            await _client.PostAsync("/game", new StringContent(string.Empty), Ct);
            var initialHitPoints = (await ReadJson(await _client.GetAsync("/game", Ct))).GetProperty("currentHitPoints").GetInt32();

            JsonElement? miss = null;
            for (var column = 0; column < 10 && miss is null; column++)
            {
                var response = await _client.PutAsJsonAsync("/game", new { column, row = 0 }, Ct);
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var json = await ReadJson(response);
                json.GetProperty("hit").ValueKind.Should().BeOneOf(JsonValueKind.True, JsonValueKind.False);
                if (!json.GetProperty("hit").GetBoolean())
                {
                    miss = json;
                }
            }

            miss.Should().NotBeNull();
            miss!.Value.GetProperty("currentHitPoints").GetInt32().Should().Be(initialHitPoints);
        }

        [Fact]
        public async Task Put_WithHit_DecreasesHitPoints()
        {
            await _client.PostAsync("/game", new StringContent(string.Empty), Ct);
            var initialHitPoints = (await ReadJson(await _client.GetAsync("/game", Ct))).GetProperty("currentHitPoints").GetInt32();

            JsonElement? hit = null;
            for (var row = 0; row < 10 && hit is null; row++)
            {
                for (var column = 0; column < 10 && hit is null; column++)
                {
                    var json = await ReadJson(await _client.PutAsJsonAsync("/game", new { column, row }, Ct));
                    if (json.GetProperty("hit").GetBoolean())
                    {
                        hit = json;
                    }
                }
            }

            hit.Should().NotBeNull();
            hit!.Value.GetProperty("currentHitPoints").GetInt32().Should().Be(initialHitPoints - 1);
        }

        private static System.Threading.CancellationToken Ct => TestContext.Current.CancellationToken;

        private static async Task<JsonElement> ReadJson(HttpResponseMessage response)
            => (await response.Content.ReadFromJsonAsync<JsonElement>(Ct));

        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
            foreach (var file in new[] { _databasePath, _databasePath + "-shm", _databasePath + "-wal" })
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            GC.SuppressFinalize(this);
        }
    }
}
