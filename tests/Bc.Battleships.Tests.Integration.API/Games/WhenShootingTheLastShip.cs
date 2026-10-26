using AwesomeAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Application.Commands;
using Bc.Battleships.Application.CommandsResults;
using Bc.Battleships.Tests.Common.Builders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.API.Games;

public class WhenShootingTheLastShip : ApiIntegrationTestBase
{
    [Fact]
    public async Task ShouldPersistWonGameInTheSameRequest()
    {
        var game = BuildGameWithOneCompartmentLeft();
        await InScope(async testContext =>
        {
            await testContext.DbContext.AddAsync(game);
            await testContext.DbContext.SaveChangesAsync();
        });

        // Column/row are zero-based in the API; the sole dry compartment is at (3, 1).
        await InScope(async testContext =>
        {
            var outcome = await testContext.Mediator.Send(new ShootCommand(game.Id, 2, 0), CancellationToken.None);
            var result = outcome.Should().BeOfType<ShootOutcome.Shot>().Subject.Result;

            result.Hit.Should().BeTrue();
            result.CurrentHitPoints.Should().Be(0);
        });

        await InScope(async testContext =>
        {
            var storedGame = await testContext.GamesRepository.GetById(game.Id, CancellationToken.None);

            storedGame.Should().NotBeNull();
            storedGame!.ComputerPlayer.CurrentHitPoints.Value.Should().Be(0);
            storedGame.GameState.Should().Be(GameState.Won);
        });
    }

    private static Game BuildGameWithOneCompartmentLeft()
    {
        var computerPlayer = new ComputerPlayerBuilder()
            .WithFleet([new ShipBuilder().WithCoordinates(1, 1).WithLength(3).Build()])
            .WithSetOfCoordinatesShot(new HashSet<Coordinates> { new(1, 1), new(2, 1) })
            .Build();

        return new GameBuilder().WithComputerPlayer(computerPlayer).Build();
    }
}
