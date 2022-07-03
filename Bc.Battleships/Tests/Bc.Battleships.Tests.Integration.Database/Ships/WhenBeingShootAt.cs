using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.Events.Ships;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Tests.Common.Builders;
using Xunit;

namespace Bc.Battleships.Tests.Integration.Database.Ships
{
    public class WhenBeingShootAt : IntegrationTestBase
    {
        [Fact]
        public void AbandonTheShipDomainEventShouldBePublishedWhenShipWasSunk()
        {
            var (ship, player) = AssumePlayerWithOneSmallShipIsCreated(1);

            player.IncomingShot(new Coordinates(1, 1));

            AssertDomainEventWasPublished<AbandonTheShipDomainEvent>(x => x.Ship == ship);
        }


        [Fact]
        public void AbandonTheShipDomainEventShouldNotBePublishedWhenShipIsAfloat()
        {
            var (_, player) = AssumePlayerWithOneSmallShipIsCreated(1);

            player.IncomingShot(new Coordinates(1, 1));

            AssertDomainEventWasNotPublished<AbandonTheShipDomainEvent>();
        }

        private static (Ship ship, ComputerPlayer player) AssumePlayerWithOneSmallShipIsCreated(int hitPoints)
        {
            var ship = new ShipBuilder()
                                    .WithLength(hitPoints)
                                    .WithCoordinates(1, 1)
                                    .Build();
            var player = new ComputerPlayerBuilder()
                .WithFleet(new[] { ship })
                .Build();

            return (ship, player);
        }

    }
}
