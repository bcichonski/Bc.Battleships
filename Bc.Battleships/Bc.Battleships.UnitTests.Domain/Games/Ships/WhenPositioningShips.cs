using FluentAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.UnitTests.Domain.Builders;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Games.Ships
{
    public class WhenPositioningShips
    {
        [Fact]
        public void ShouldPositionShipHorizontaly()
        {
            var ship = new ShipBuilder()
                .WithCoordinates(1, 1)
                .WithLayout(ShipLayout.Horizontal)
                .WithLength(3)
                .Build();

            ship.Compartments.Select(x => x.Coordinates.X).Should().BeEquivalentTo(new[] { 1, 2, 3 });
            ship.Compartments.Select(x => x.Coordinates.Y).Should().AllSatisfy(x => x.Should().Be(1));
        }

        [Fact]
        public void ShouldPositionShipVertically()
        {
            var ship = new ShipBuilder()
                .WithCoordinates(1, 1)
                .WithLayout(ShipLayout.Vertical)
                .WithLength(3)
                .Build();

            ship.Compartments.Select(x => x.Coordinates.Y).Should().BeEquivalentTo(new[] { 1, 2, 3 });
            ship.Compartments.Select(x => x.Coordinates.X).Should().AllSatisfy(x => x.Should().Be(1));
        }

        [Theory]
        [MemberData(nameof(CollisionData))]
        public void ShouldHandleColisions(Ship ship1, Ship ship2, bool expectedResult)
        {
            ship1.CollidesWith(ship2).Should().Be(expectedResult);
            ship2.CollidesWith(ship1).Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> CollisionData =>
            new List<object[]>
            {
                new object[] {
                    new ShipBuilder()
                        .Build(),
                    new ShipBuilder()
                        .Build(),
                    true
                },
                new object[] {
                    new ShipBuilder()
                        .WithLayout(ShipLayout.Horizontal)
                        .WithCoordinates(1,1)
                        .WithLength(3)
                        .Build(),
                    new ShipBuilder()
                        .WithLayout(ShipLayout.Horizontal)
                        .WithCoordinates(1,2)
                        .WithLength(3)
                        .Build(),
                    false
                },
                new object[] {
                    new ShipBuilder()
                        .WithLayout(ShipLayout.Horizontal)
                        .WithCoordinates(1,2)
                        .WithLength(3)
                        .Build(),
                    new ShipBuilder()
                        .WithLayout(ShipLayout.Vertical)
                        .WithCoordinates(2,1)
                        .WithLength(3)
                        .Build(),
                    true
                },
            };
    }
}
