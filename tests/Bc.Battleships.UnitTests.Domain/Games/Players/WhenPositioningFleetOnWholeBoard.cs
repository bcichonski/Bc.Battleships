using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.Services.FleetFactory;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.GameContext;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Games.Players
{
    public class WhenPositioningFleetOnWholeBoard
    {
        private const int Seeds = 1000;
        private static readonly GameContext StandardContext = new(new Length(10));

        private static ComputerPlayer Position(GameContext context, int seed)
        {
            var player = new ComputerPlayer();
            player.PositionShips(context, new Random(seed));
            return player;
        }

        [Fact]
        public void EveryCellShouldBeReachableAcrossManySeeds()
        {
            var occupied = new HashSet<Coordinates>();

            for (var seed = 0; seed < Seeds; seed++)
            {
                foreach (var ship in Position(StandardContext, seed).Ships!)
                {
                    foreach (var compartment in ship.Compartments)
                    {
                        occupied.Add(compartment.Coordinates);
                    }
                }
            }

            var all = Enumerable.Range(1, 10).SelectMany(c => Enumerable.Range(1, 10).Select(r => new Coordinates(c, r)));
            all.Where(x => !occupied.Contains(x)).Should().BeEmpty();
        }

        [Fact]
        public void ShipsShouldBeOnBoardAndNotIntersectForEverySeed()
        {
            for (var seed = 0; seed < Seeds; seed++)
            {
                var ships = Position(StandardContext, seed).Ships!;

                ships.Should().HaveCount(3);
                ships.SelectMany(s => s.Compartments).Should().AllSatisfy(c =>
                {
                    c.Coordinates.Column.Should().BeInRange(1, 10);
                    c.Coordinates.Row.Should().BeInRange(1, 10);
                });
                ships.SelectMany(s => s.Compartments.Select(c => c.Coordinates)).Distinct().Should().HaveCount(13);
            }
        }

        [Fact]
        public void SameSeedShouldGiveSameFleet()
        {
            for (var seed = 0; seed < 50; seed++)
            {
                var first = Position(StandardContext, seed).Ships!
                    .Select(s => (s.Layout, s.Coordinates)).ToList();
                var second = Position(StandardContext, seed).Ships!
                    .Select(s => (s.Layout, s.Coordinates)).ToList();

                second.Should().Equal(first);
            }
        }

        [Fact]
        public void ShipLongerThanBoardShouldThrow()
        {
            var context = new GameContext(new Length(4));

            var act = () => Position(context, 1);

            act.Should().Throw<FleetFactoryPositioningFailedException>();
        }
    }
}
