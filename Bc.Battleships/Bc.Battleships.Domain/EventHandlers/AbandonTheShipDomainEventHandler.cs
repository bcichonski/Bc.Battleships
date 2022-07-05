using Bc.Battleships.Domain.Events.Ships;
using Bc.Battleships.Domain.Repositories;
using MediatR;

namespace Bc.Battleships.Domain.EventHandlers
{
    public class AbandonTheShipDomainEventHandler : INotificationHandler<AbandonTheShipDomainEvent>
    {
        private readonly IGamesRepository _gamesRepository;

        public AbandonTheShipDomainEventHandler(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }

        public async Task Handle(AbandonTheShipDomainEvent notification, CancellationToken cancellationToken)
        {
            if(notification.GameId == null)
            {
                return;
            }

            var game = await _gamesRepository.GetById(notification.GameId, cancellationToken);

            if(game?.ComputerPlayer.CurrentHitPoints.Value <= 0)
            {
                game.Stop();
            }
        }
    }
}
