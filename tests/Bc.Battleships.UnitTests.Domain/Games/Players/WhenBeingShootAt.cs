using AwesomeAssertions;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Tests.Common.Builders;
using System.Linq;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Games.Players;

public class WhenBeingShootAt
{
    [Fact]
    public void CurrentHitPointsShouldBeDecreasedOnHit()
    {
        var player = new ComputerPlayerBuilder()
            .Build();
        var playerHitPoints = player.CurrentHitPoints;

        player.IncomingShot(new Coordinates(2, 1));

        player.CurrentHitPoints.Should().Be(playerHitPoints.Substract(1));
    }

    [Fact]
    public void CurrentHitPointsShouldBeSameOnMiss()
    {
        var player = new ComputerPlayerBuilder()
            .Build();
        var playerHitPoints = player.CurrentHitPoints;

        player.IncomingShot(new Coordinates(2, 3));

        player.CurrentHitPoints.Should().Be(playerHitPoints);
    }

    [Fact]
    public void CoordinatesOfTheShootShouldBeSaved()
    {
        var player = new ComputerPlayerBuilder()
            .Build();

        var shotCoordinates = new Coordinates(2, 1);

        player.IncomingShot(shotCoordinates);

        player.CoordinatesShoot.FirstOrDefault(x => x.Coordinates == shotCoordinates).Should().NotBeNull();
    }

    [Fact]
    public void CoordinatesOfTheShootShouldBeSavedOnlyOnce()
    {
        var player = new ComputerPlayerBuilder()
            .Build();

        var shotCoordinates = new Coordinates(2, 1);

        player.IncomingShot(shotCoordinates);
        player.IncomingShot(shotCoordinates);

        player.CoordinatesShoot.Count(x => x.Coordinates == shotCoordinates).Should().Be(1);
    }

    [Fact]
    public void RepeatShotOnHitCompartmentShouldReturnAlreadyShotAndKeepHitPoints()
    {
        var player = new ComputerPlayerBuilder()
            .Build();
        var playerHitPoints = player.CurrentHitPoints;

        var result = player.IncomingShot(new Coordinates(1, 1));

        result.Should().Be(ShootResult.AlreadyShot);
        player.CurrentHitPoints.Should().Be(playerHitPoints);
    }

    [Fact]
    public void RepeatShotOnWaterShouldReturnAlreadyShot()
    {
        var player = new ComputerPlayerBuilder()
            .Build();
        var shotCoordinates = new Coordinates(2, 3);
        player.IncomingShot(shotCoordinates);

        var result = player.IncomingShot(shotCoordinates);

        result.Should().Be(ShootResult.AlreadyShot);
        player.CoordinatesShoot.Count(x => x.Coordinates == shotCoordinates).Should().Be(1);
    }
}
