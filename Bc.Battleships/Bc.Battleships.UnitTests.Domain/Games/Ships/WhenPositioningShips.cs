using FluentAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.UnitTests.Domain.Builders;
using System.Collections.Generic;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Games.Ships
{
    public class WhenPositioningShips
    {
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
                        .WithCoordinates(new Coordinates(1,1))
                        .WithLength(new Length(3))
                        .Build(),
                    new ShipBuilder()
                        .WithLayout(ShipLayout.Horizontal)
                        .WithCoordinates(new Coordinates(1,2))
                        .WithLength(new Length(3))
                        .Build(),
                    false
                },
                new object[] {
                    new ShipBuilder()
                        .WithLayout(ShipLayout.Horizontal)
                        .WithCoordinates(new Coordinates(2,1))
                        .WithLength(new Length(3))
                        .Build(),
                    new ShipBuilder()
                        .WithLayout(ShipLayout.Vertical)
                        .WithCoordinates(new Coordinates(1,2))
                        .WithLength(new Length(3))
                        .Build(),
                    true
                },
            };
    }
}
