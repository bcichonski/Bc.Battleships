using FluentAssertions;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Persistence.Repositories;
using Bc.Battleships.Tests.Common.Builders;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.Database.Games
{
    public class WhenBeingShootAt : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldStoreShotCoordinates()
        {
            await AssumeGameInDb();
            var expectedCoordinates = new Coordinates(2, 2);

            var gameId = await WhenShotIsMade(expectedCoordinates);

            await AssertCoordinatesOfTheShootingWereSaved(gameId, expectedCoordinates);
        }

        private async Task AssertCoordinatesOfTheShootingWereSaved(GameId gameId, Coordinates cooridnates)
        {
            await InScope(async dbContext =>
            {
                var repository = new GamesRepository(dbContext);

                var game = await repository.GetById(gameId, CancellationToken.None);

                game.Should().NotBeNull();
                game!.ComputerPlayer.CoordinatesShoot.FirstOrDefault(x => x.Coordinates == cooridnates).Should().NotBeNull();
            });
        }

        private async Task<GameId> WhenShotIsMade(Coordinates cooridnates)
        {
            GameId? gameId = default;
            await InScope(async dbContext =>
            {
                var repository = new GamesRepository(dbContext);

                var game = await repository.Get(CancellationToken.None);

                game!.Shoot(cooridnates);

                await dbContext.SaveChangesAsync();

                gameId = game.Id;
            });

            return gameId!;
        }

        private Task AssumeGameInDb()
            => InScope(async dbContext =>
            {
                await dbContext!.AddAsync(new GameBuilder().Build());

                await dbContext.SaveChangesAsync();
            });
    }
}
