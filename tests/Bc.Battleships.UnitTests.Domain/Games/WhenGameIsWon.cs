using AwesomeAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Tests.Common.Builders;
using System.Collections.Generic;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Games
{
    public class WhenGameIsWon
    {
        [Fact]
        public void ShouldBeWonWhenLastShipIsSunk()
        {
            var game = BuildGame(
                [new ShipBuilder().WithCoordinates(1, 1).WithLength(3).Build()],
                shot: [new Coordinates(1, 1), new Coordinates(2, 1)]);

            game.Shoot(new Coordinates(3, 1));

            game.GameState.Should().Be(GameState.Won);
            game.ComputerPlayer.CurrentHitPoints.Value.Should().Be(0);
        }

        [Fact]
        public void ShouldStayActiveWhenShipIsSunkWhileAnotherIsAfloat()
        {
            var game = BuildGame(
                [
                    new ShipBuilder().WithCoordinates(1, 1).WithLength(1).Build(),
                    new ShipBuilder().WithCoordinates(1, 3).WithLength(2).Build()
                ],
                shot: []);

            game.Shoot(new Coordinates(1, 1));

            game.GameState.Should().Be(GameState.Active);
            game.ComputerPlayer.CurrentHitPoints.Value.Should().Be(2);
        }

        [Fact]
        public void ShouldStayActiveWhenNonFinalCompartmentIsHit()
        {
            var game = BuildGame([new ShipBuilder().WithCoordinates(1, 1).WithLength(3).Build()], shot: []);

            game.Shoot(new Coordinates(2, 1));

            game.GameState.Should().Be(GameState.Active);
        }

        [Fact]
        public void ShouldStayActiveWhenShotMisses()
        {
            var game = BuildGame([new ShipBuilder().WithCoordinates(1, 1).WithLength(3).Build()], shot: []);

            game.Shoot(new Coordinates(5, 5));

            game.GameState.Should().Be(GameState.Active);
        }

        [Fact]
        public void ShouldStayWonWhenShootingAWonGame()
        {
            var game = BuildGame(
                [new ShipBuilder().WithCoordinates(1, 1).WithLength(1).Build()],
                shot: []);
            game.Shoot(new Coordinates(1, 1));

            game.Shoot(new Coordinates(5, 5));
            game.Shoot(new Coordinates(1, 1));

            game.GameState.Should().Be(GameState.Won);
        }

        [Fact]
        public void ShouldStayActiveWhenHitCompartmentIsShotAgain()
        {
            var game = BuildGame([new ShipBuilder().WithCoordinates(1, 1).WithLength(3).Build()], shot: [new Coordinates(1, 1)]);

            var result = game.Shoot(new Coordinates(1, 1));

            result.Should().Be(ShootResult.AlreadyShot);
            game.GameState.Should().Be(GameState.Active);
            game.ComputerPlayer.CurrentHitPoints.Value.Should().Be(2);
        }

        [Fact]
        public void ShouldReportSunkAndBeWonWhenLastCompartmentIsFlooded()
        {
            var game = BuildGame([new ShipBuilder().WithCoordinates(1, 1).WithLength(2).Build()], shot: [new Coordinates(1, 1)]);

            var result = game.Shoot(new Coordinates(2, 1));

            result.Should().Be(ShootResult.Sunk);
            game.GameState.Should().Be(GameState.Won);
        }

        [Fact]
        public void ShouldStayWonWhenSunkShipIsShotAgain()
        {
            var game = BuildGame([new ShipBuilder().WithCoordinates(1, 1).WithLength(1).Build()], shot: []);
            game.Shoot(new Coordinates(1, 1));

            var result = game.Shoot(new Coordinates(1, 1));

            result.Should().Be(ShootResult.AlreadyShot);
            game.GameState.Should().Be(GameState.Won);
        }

        private static Game BuildGame(IList<Bc.Battleships.Domain.AggregateRoots.Games.Ships.Ship> ships, IEnumerable<Coordinates> shot)
        {
            var computerPlayer = new ComputerPlayerBuilder()
                .WithFleet(ships)
                .WithSetOfCoordinatesShot(new HashSet<Coordinates>(shot))
                .Build();

            return new GameBuilder()
                .WithComputerPlayer(computerPlayer)
                .Build();
        }
    }
}
