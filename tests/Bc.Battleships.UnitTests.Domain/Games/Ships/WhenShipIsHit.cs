using AwesomeAssertions;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Tests.Common.Builders;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Games.Ships;

public class WhenShipIsHit
{
    [Fact]
    public void ShouldReturnMissWhenNoCompartmentAtCoordinates()
    {
        var ship = new ShipBuilder().WithCoordinates(1, 1).WithLength(3).Build();

        var result = ship.Hit(new Coordinates(5, 5));

        result.Should().Be(ShootResult.Miss);
        ship.CurrentHitPoints.Value.Should().Be(3);
    }

    [Fact]
    public void ShouldReturnHitWhenDryCompartmentIsFlooded()
    {
        var ship = new ShipBuilder().WithCoordinates(1, 1).WithLength(3).Build();

        var result = ship.Hit(new Coordinates(2, 1));

        result.Should().Be(ShootResult.Hit);
        ship.CurrentHitPoints.Value.Should().Be(2);
    }

    [Fact]
    public void ShouldReturnAlreadyShotWhenFloodedCompartmentIsHitAgain()
    {
        var ship = new ShipBuilder().WithCoordinates(1, 1).WithLength(3).Build();
        ship.Hit(new Coordinates(2, 1));

        var result = ship.Hit(new Coordinates(2, 1));

        result.Should().Be(ShootResult.AlreadyShot);
        ship.CurrentHitPoints.Value.Should().Be(2);
    }

    [Fact]
    public void ShouldReturnSunkWhenLastDryCompartmentIsFlooded()
    {
        var ship = new ShipBuilder().WithCoordinates(1, 1).WithLength(2).Build();
        ship.Hit(new Coordinates(1, 1));

        var result = ship.Hit(new Coordinates(2, 1));

        result.Should().Be(ShootResult.Sunk);
        ship.CurrentHitPoints.Value.Should().Be(0);
    }

    [Fact]
    public void ShouldReturnAlreadyShotWhenSunkShipIsHitAgain()
    {
        var ship = new ShipBuilder().WithCoordinates(1, 1).WithLength(1).Build();
        ship.Hit(new Coordinates(1, 1));

        var result = ship.Hit(new Coordinates(1, 1));

        result.Should().Be(ShootResult.AlreadyShot);
    }
}
