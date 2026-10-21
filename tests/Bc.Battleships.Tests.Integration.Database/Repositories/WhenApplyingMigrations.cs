using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Persistence;
using Bc.Battleships.Persistence.Infrastructure;
using Bc.Battleships.Persistence.Providers;
using Bc.Battleships.Tests.Common.Builders;
using AwesomeAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.Database.Repositories
{
    public class WhenApplyingMigrations : IDisposable
    {
        private readonly string _databaseFile = Path.Combine(Path.GetTempPath(), $"battleships.migrations.{Path.GetRandomFileName()}.db");
        private readonly ServiceProvider _serviceProvider;

        public WhenApplyingMigrations()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IPersistenceConfigurationProvider>(new FixedConfigurationProvider($"Data Source = {_databaseFile}"));
            Services.Configure(services);
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
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                await scope.ServiceProvider.GetRequiredService<DatabaseMigrator>().ApplyMigrationsIfNeeded();
                await scope.ServiceProvider.GetRequiredService<IGamesRepository>().Add(new GameBuilder().Build());
            }

            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var game = await scope.ServiceProvider.GetRequiredService<IGamesRepository>().Get(CancellationToken.None);

                game.Should().NotBeNull();
                game!.Id.Value.Should().NotBeEmpty();
                game.ComputerPlayer.Should().NotBeNull();
                game.HumanPlayer.Should().NotBeNull();
            }
        }

        public void Dispose()
        {
            _serviceProvider.Dispose();
            SqliteConnection.ClearAllPools();
            File.Delete(_databaseFile);
            GC.SuppressFinalize(this);
        }

        private sealed class FixedConfigurationProvider : IPersistenceConfigurationProvider
        {
            public FixedConfigurationProvider(string connectionString) => ConnectionString = connectionString;

            public string ConnectionString { get; }
        }
    }
}
