using AwesomeAssertions;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Tests.Common.Builders;
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
    }
}
