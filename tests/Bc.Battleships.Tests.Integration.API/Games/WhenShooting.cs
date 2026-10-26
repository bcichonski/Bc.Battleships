using AwesomeAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Application.Commands;
using Bc.Battleships.Application.CommandsResults;
using System;
using System.Linq;
using Bc.Battleships.Tests.Common.Builders;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using GameState = Bc.Battleships.Domain.ValueObjects.Enums.GameState;

namespace Bc.Battleships.Tests.Integration.API.Games;

public class WhenShooting : ApiIntegrationTestBase
{
    [Fact]
    public async Task ShouldReturnHit()
    {
        var game = await AssumeGameInDb();
        var currentHitPoints = game.ComputerPlayer.CurrentHitPoints.Value;
        var shootCommand = new ShootCommand(game.Id, 1, 0);

        await InScope(async testContext =>
        {
            var result = ShotResultOf(await testContext.Mediator.Send(shootCommand, CancellationToken.None));

            result.Hit.Should().BeTrue();
            result.CurrentHitPoints.Should().Be(currentHitPoints - 1);
        });
    }

    [Fact]
    public async Task ShouldReturnMiss()
    {
        var game = await AssumeGameInDb();
        var currentHitPoints = game.ComputerPlayer.CurrentHitPoints.Value;
        var expectedCoordinates = new Coordinates(3, 3);
        var shootCommand = new ShootCommand(game.Id, expectedCoordinates.Column, expectedCoordinates.Row);

        await InScope(async testContext =>
        {
            var result = ShotResultOf(await testContext.Mediator.Send(shootCommand, CancellationToken.None));

            result.Hit.Should().BeFalse();
            result.CurrentHitPoints.Should().Be(currentHitPoints);
        });
    }

    [Fact]
    public async Task ShouldReturnHitWithUnchangedHitPointsWhenHitCellIsShotAgain()
    {
        var game = await AssumeGameInDb();
        var expectedHitPoints = game.ComputerPlayer.CurrentHitPoints.Value - 1;
        var shootCommand = new ShootCommand(game.Id, 1, 0);

        await InScope(async testContext =>
            await testContext.Mediator.Send(shootCommand, CancellationToken.None));

        await InScope(async testContext =>
        {
            var result = ShotResultOf(await testContext.Mediator.Send(shootCommand, CancellationToken.None));

            result.Hit.Should().BeTrue();
            result.CurrentHitPoints.Should().Be(expectedHitPoints);
        });
    }

    [Fact]
    public async Task ShouldReturnMissWithUnchangedHitPointsWhenWaterIsShotAgain()
    {
        var game = await AssumeGameInDb();
        var currentHitPoints = game.ComputerPlayer.CurrentHitPoints.Value;
        var shootCommand = new ShootCommand(game.Id, 3, 3);

        await InScope(async testContext =>
            await testContext.Mediator.Send(shootCommand, CancellationToken.None));

        await InScope(async testContext =>
        {
            var result = ShotResultOf(await testContext.Mediator.Send(shootCommand, CancellationToken.None));

            result.Hit.Should().BeFalse();
            result.CurrentHitPoints.Should().Be(currentHitPoints);
        });
    }

    [Fact]
    public async Task ShouldReportGameNotFoundForUnknownId()
    {
        await AssumeGameInDb();

        await InScope(async testContext =>
        {
            var outcome = await testContext.Mediator.Send(new ShootCommand(new GameId(Guid.NewGuid()), 0, 0), CancellationToken.None);

            outcome.Should().BeOfType<ShootOutcome.GameNotFound>();
        });
    }

    [Fact]
    public async Task ShouldRejectShotOnStoppedGameWithoutRecordingIt()
    {
        var game = await AssumeGameInDb();
        await InScope(async testContext =>
        {
            var stored = await testContext.GamesRepository.GetById(game.Id, CancellationToken.None);
            stored!.Stop();
            await testContext.DbContext.SaveChangesAsync();
        });

        await InScope(async testContext =>
        {
            var outcome = await testContext.Mediator.Send(new ShootCommand(game.Id, 3, 3), CancellationToken.None);

            outcome.Should().BeOfType<ShootOutcome.GameNotActive>();
        });

        await InScope(async testContext =>
        {
            var stored = await testContext.GamesRepository.GetById(game.Id, CancellationToken.None);

            var storedShots = stored!.ComputerPlayer.CoordinatesShoot.Select(x => x.Coordinates);

            storedShots.Should().NotContain(new Coordinates(4, 4));
            storedShots.Should().BeEquivalentTo(game.ComputerPlayer.CoordinatesShoot.Select(x => x.Coordinates));
            stored.GameState.Should().Be(GameState.Ended);
        });
    }

    private static ShootResult ShotResultOf(ShootOutcome outcome)
        => outcome.Should().BeOfType<ShootOutcome.Shot>().Subject.Result;
}
