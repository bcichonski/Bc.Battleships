using AwesomeAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Infrastructure.Repositories;
using Bc.Battleships.Tests.Common.Builders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.Database.Games;

public class WhenGameIsWon : IntegrationTestBase
{
    [Fact]
    public async Task ShouldStoreAndReloadWonState()
    {
        var gameId = await AssumeWonGameInDb();

        await InScope(async dbContext =>
        {
            var game = await new GamesRepository(dbContext).GetById(gameId, CancellationToken.None);

            game.Should().NotBeNull();
            game!.GameState.Should().Be(GameState.Won);
        });
    }

    private async Task<GameId> AssumeWonGameInDb()
    {
        var computerPlayer = new ComputerPlayerBuilder()
            .WithFleet([new ShipBuilder().WithCoordinates(1, 1).WithLength(3).Build()])
            .WithSetOfCoordinatesShot(new HashSet<Coordinates> { new(1, 1), new(2, 1) })
            .Build();
        var game = new GameBuilder().WithComputerPlayer(computerPlayer).Build();
        game.Shoot(new Coordinates(3, 1));

        await InScope(async dbContext =>
        {
            await dbContext.AddAsync(game);
            await dbContext.SaveChangesAsync();
        });

        return game.Id;
    }
}
