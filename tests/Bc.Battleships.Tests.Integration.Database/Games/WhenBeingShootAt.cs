using AwesomeAssertions;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Infrastructure.Repositories;
using Bc.Battleships.Tests.Common.Builders;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.Database.Games;

public class WhenBeingShootAt : IntegrationTestBase
{
    [Fact]
    public async Task ShouldStoreShotCoordinates()
    {
        var gameId = await AssumeGameInDb();
        var expectedCoordinates = new Coordinates(2, 2);

        await WhenShotIsMade(gameId, expectedCoordinates);

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

    private Task WhenShotIsMade(GameId gameId, Coordinates cooridnates)
        => InScope(async dbContext =>
        {
            var repository = new GamesRepository(dbContext);

            var game = await repository.GetById(gameId, CancellationToken.None);

            game!.Shoot(cooridnates);

            await dbContext.SaveChangesAsync();
        });

    private async Task<GameId> AssumeGameInDb()
    {
        var game = new GameBuilder().Build();

        await InScope(async dbContext =>
        {
            await dbContext!.AddAsync(game);

            await dbContext.SaveChangesAsync();
        });

        return game.Id;
    }
}
