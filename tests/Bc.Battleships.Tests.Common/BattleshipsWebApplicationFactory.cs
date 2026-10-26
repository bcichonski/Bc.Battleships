using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Bc.Battleships.Tests.Common;

/// <summary>
/// Web application factory backed by its own <see cref="TestDatabase"/>; the host applies the migrations on startup.
/// </summary>
public sealed class BattleshipsWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly TestDatabase _database = new("endpoints");
    private readonly List<IDisposable> _derivedFactories = [];

    public TestDatabase Database => _database;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => builder.ConfigureAppConfiguration((_, configuration) =>
            configuration.AddInMemoryCollection(
            [
                new KeyValuePair<string, string?>("ConnectionStrings:ConnectionString", _database.ConnectionString)
            ]));

    public HttpClient CreateClient(int boardSize, int[] shipLengths)
    {
        var settings = new List<KeyValuePair<string, string?>> { new("Game:BoardSize", boardSize.ToString()) };
        settings.AddRange(shipLengths.Select((length, index) =>
            new KeyValuePair<string, string?>($"Game:ShipLengths:{index}", length.ToString())));

        var factory = WithWebHostBuilder(builder =>
            builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(settings)));
        _derivedFactories.Add(factory);

        return factory.CreateClient();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var factory in _derivedFactories)
            {
                factory.Dispose();
            }
        }

        base.Dispose(disposing);

        if (disposing)
        {
            _database.Dispose();
        }
    }
}
