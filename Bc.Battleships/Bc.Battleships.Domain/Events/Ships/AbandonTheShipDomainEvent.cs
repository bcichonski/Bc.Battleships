using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.ValueObjects;

namespace Bc.Battleships.Domain.Events.Ships
{
    public class AbandonTheShipDomainEvent : IDomainEvent
    {
        public GameId? GameId { get; }

        public AbandonTheShipDomainEvent(GameId? gameId)
        {
            GameId = gameId;
        }
    }
}
