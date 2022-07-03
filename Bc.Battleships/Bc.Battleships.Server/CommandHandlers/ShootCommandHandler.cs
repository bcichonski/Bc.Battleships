using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Server.Commands;
using Bc.Battleships.Server.CommandsResults;
using MediatR;
using ShootResultEnum = Bc.Battleships.Domain.ValueObjects.Enums.ShootResult;

namespace Bc.Battleships.Server.CommandHandlers
{
    internal class ShootCommandHandler : IRequestHandler<ShootCommand, ShootResult>
    {
        private readonly IGamesRepository _gamesRepository;

        public ShootCommandHandler(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }

        public async Task<ShootResult> Handle(ShootCommand request, CancellationToken cancellationToken)
        {
            var game = await _gamesRepository.Get(cancellationToken);

            var result = game!.Shoot(request.Coordinates);

            return new ShootResult(result == ShootResultEnum.Hit, game.ComputerPlayer.CurrentHitPoints.Value);
        }
    }
}
