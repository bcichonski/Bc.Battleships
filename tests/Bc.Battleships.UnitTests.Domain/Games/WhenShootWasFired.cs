using AwesomeAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Tests.Common.Builders;
using System.Collections.Generic;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Games
{
    public class WhenShootWasFired
    {
        [Fact]
        public void ShouldMiss()
        {
            var game = new GameBuilder().Build();

            var result = game.Shoot(new Coordinates(2, 4));

            result.Should().Be(ShootResult.Miss);
        }

        [Fact]
        public void ShouldHit()
        {
            var game = new GameBuilder().Build();

            var result = game.Shoot(new Coordinates(2, 1));

            result.Should().Be(ShootResult.Hit);
        }

        [Fact]
        public void ShouldReturnAlreadyShotWhenHitCompartmentIsShotAgain()
        {
            var game = BuildGame([new ShipBuilder().WithCoordinates(1, 1).WithLength(3).Build()]);
            game.Shoot(new Coordinates(2, 1));
            var hitPoints = game.ComputerPlayer.CurrentHitPoints;

            var result = game.Shoot(new Coordinates(2, 1));

            result.Should().Be(ShootResult.AlreadyShot);
            game.ComputerPlayer.CurrentHitPoints.Should().Be(hitPoints);
        }

        [Fact]
        public void ShouldReturnAlreadyShotWhenWaterIsShotAgain()
        {
            var game = BuildGame([new ShipBuilder().WithCoordinates(1, 1).WithLength(3).Build()]);
            game.Shoot(new Coordinates(5, 5));
            var hitPoints = game.ComputerPlayer.CurrentHitPoints;

            var result = game.Shoot(new Coordinates(5, 5));

            result.Should().Be(ShootResult.AlreadyShot);
            game.ComputerPlayer.CurrentHitPoints.Should().Be(hitPoints);
        }

        [Fact]
        public void ShouldReturnSunkWhenLastDryCompartmentIsFlooded()
        {
            var game = BuildGame(
                [
                    new ShipBuilder().WithCoordinates(1, 1).WithLength(2).Build(),
                    new ShipBuilder().WithCoordinates(1, 3).WithLength(2).Build()
                ]);
            game.Shoot(new Coordinates(1, 1));

            var result = game.Shoot(new Coordinates(2, 1));

            result.Should().Be(ShootResult.Sunk);
        }

        [Fact]
        public void ShouldReturnAlreadyShotWhenSunkShipIsShotAgain()
        {
            var game = BuildGame(
                [
                    new ShipBuilder().WithCoordinates(1, 1).WithLength(1).Build(),
                    new ShipBuilder().WithCoordinates(1, 3).WithLength(2).Build()
                ]);
            game.Shoot(new Coordinates(1, 1));

            var result = game.Shoot(new Coordinates(1, 1));

            result.Should().Be(ShootResult.AlreadyShot);
        }

        private static Game BuildGame(IList<Ship> ships)
        {
            var computerPlayer = new ComputerPlayerBuilder()
                .WithFleet(ships)
                .WithSetOfCoordinatesShot(new HashSet<Coordinates>())
                .Build();

            return new GameBuilder()
                .WithComputerPlayer(computerPlayer)
                .Build();
        }
    }
}
