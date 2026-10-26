using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Infrastructure;
using Bc.Battleships.Infrastructure.Infrastructure;
using Bc.Battleships.Infrastructure.Providers;
using Bc.Battleships.Tests.Common;
using Bc.Battleships.Tests.Common.Builders;
using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.Database.Repositories;

public class WhenApplyingMigrations : IDisposable
{
    private readonly TestDatabase _database = new("migrations");
    private readonly ServiceProvider _serviceProvider;

    public WhenApplyingMigrations()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IPersistenceConfigurationProvider>(new FixedConfigurationProvider(_database.ConnectionString));
        Bc.Battleships.Infrastructure.Infrastructure.Services.Configure(services);
        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task ShouldApplyMigrationsToEmptyDatabaseWithoutPendingModelChanges()
    {
        await using (var scope = _serviceProvider.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BattleshipsDbContext>();

            await scope.ServiceProvider.GetRequiredService<DatabaseMigrator>().ApplyMigrationsIfNeeded();

            (await dbContext.Database.GetPendingMigrationsAsync(TestContext.Current.CancellationToken)).Should().BeEmpty();
            (await dbContext.Database.GetAppliedMigrationsAsync(TestContext.Current.CancellationToken)).Should().NotBeEmpty();
            dbContext.Database.HasPendingModelChanges().Should().BeFalse();
        }
    }

    [Fact]
    public async Task ShouldRoundTripGameThroughMigratedDatabase()
    {
        var newGame = new GameBuilder().Build();
        await using (var scope = _serviceProvider.CreateAsyncScope())
        {
            await scope.ServiceProvider.GetRequiredService<DatabaseMigrator>().ApplyMigrationsIfNeeded();
            scope.ServiceProvider.GetRequiredService<IGamesRepository>().Add(newGame);
            await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().SaveChangesAsync(CancellationToken.None);
        }

        await using (var scope = _serviceProvider.CreateAsyncScope())
        {
            var game = await scope.ServiceProvider.GetRequiredService<IGamesRepository>().GetById(newGame.Id, CancellationToken.None);

            game.Should().NotBeNull();
            game!.Id.Value.Should().NotBeEmpty();
            game.ComputerPlayer.Should().NotBeNull();
            game.HumanPlayer.Should().NotBeNull();
        }
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
        _database.Dispose();
        GC.SuppressFinalize(this);
    }

    private sealed class FixedConfigurationProvider(string connectionString) : IPersistenceConfigurationProvider
    {
        public string ConnectionString { get; } = connectionString;
    }
}
