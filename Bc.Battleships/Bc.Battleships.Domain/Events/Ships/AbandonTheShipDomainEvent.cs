using Bc.Battleships.Domain.AggregateRoots.Games.Ships;

namespace Bc.Battleships.Domain.Events.Ships
{
    public class AbandonTheShipDomainEvent : IDomainEvent
    {
        public Ship Ship { get; }

        public AbandonTheShipDomainEvent(Ship ship)
        {
            Ship = ship;
        }
    }
}
