using FluentAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Tests.Common.Builders;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.Database.Repositories
{
    public class WhenGettingGame : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldReturnGame()
        {
            await AssumeGameInDb();

            await InScope(async (_) =>
            {
                var gameRepository = ServiceProvider.GetService<IGamesRepository>();
                var game = await gameRepository!.Get(CancellationToken.None);

                game!.Should().NotBeNull();
                game!.Id.Value.Should().NotBeEmpty();
            });
        }

        [Fact]
        public async Task ShouldPreservePlayerTypes()
        {
            await AssumeGameInDb();

            await InScope(async (_) =>
            {
                var gameRepository = ServiceProvider.GetService<IGamesRepository>();
                var game = await gameRepository!.Get(CancellationToken.None);

                game!.ComputerPlayer.Should().NotBeNull();
                game!.HumanPlayer.Should().NotBeNull();
            });
        }

        [Fact]
        public async Task ShouldPreserveFleetState()
        {
            await AssumeGameInProgressInDb();

            await InScope(async (_) =>
            {
                var gameRepository = ServiceProvider.GetService<IGamesRepository>();
                var game = await gameRepository!.Get(CancellationToken.None);

                game!.ComputerPlayer.CurrentHitPoints.Value.Should().BeGreaterThan(0);
            });
        }

        private Task AssumeGameInProgressInDb()
             => InScope(async dbContext =>
                 {
                     var game = new GameBuilder().Build();

                     await dbContext!.AddAsync(game);
                     await dbContext.SaveChangesAsync();
                 });

        private Task AssumeGameInDb()
            => InScope(async dbContext =>
                {
                    await dbContext!.AddAsync(new GameBuilder().Build());
                    await dbContext.SaveChangesAsync();
                });
    }
}