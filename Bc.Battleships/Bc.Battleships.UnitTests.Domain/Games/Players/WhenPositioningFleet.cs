using FluentAssertions;
using Bc.Battleships.Domain.Services;
using Bc.Battleships.Tests.Common.Builders;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Games.Players
{
    public class WhenPositioningFleet
    {
        [Fact]
        public void AllShipsShouldBePositioned()
        {
            var player = new ComputerPlayerBuilder()
                .WithNoFleet()
                .Build();

            player.PositionShips();

            player.Ships.Should().NotBeNull();
            player.Ships.Should().AllSatisfy(x => x.Compartments.Should().NotBeNull());
        }
    }
}
