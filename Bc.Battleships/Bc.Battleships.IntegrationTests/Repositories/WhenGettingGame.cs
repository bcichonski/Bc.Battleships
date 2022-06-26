using FluentAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.IntegrationTests.Repositories
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
                var game = await gameRepository!.Get();

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
                var game = await gameRepository!.Get();

                game!.ComputerPlayer.Should().NotBeNull();
                game!.HumanPlayer.Should().NotBeNull();
            });
        }

        private Task AssumeGameInDb()
            => InScope(async dbContext =>
                {
                    await dbContext!.AddAsync(new Game());
                    await dbContext.SaveChangesAsync();
                });
    }
}