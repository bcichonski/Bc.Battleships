using FluentAssertions;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Tests.Common.Builders;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Games.Players
{
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
    }
}
