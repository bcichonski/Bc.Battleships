using FluentAssertions;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.UnitTests.Domain.Builders;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Games.Ships
{
    public class WhenCreatingShip
    {
        [Fact]
        public void ItShouldBeInitialized()
        {
            var ship = new ShipBuilder()
                .WithLayout(ShipLayout.Vertical)
                .Build();

            ship.Coordinates.Should().NotBeNull();
            ship.Length.Should().NotBeNull();
            ship.Layout.Should().Be(ShipLayout.Vertical);
        }

        [Fact]
        public void ItShouldHaveCompartments()
        {
            var ship = new ShipBuilder()
                .Build();

            ship.Compartments.Should().NotBeNull();
            ship.Compartments.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public void ItShouldHavePositiveHitPointsValue()
        {
            var ship = new ShipBuilder()
                .Build();

            ship.CurrentHitPoints.Value.Should().BeGreaterThan(0);
        }
    }
}