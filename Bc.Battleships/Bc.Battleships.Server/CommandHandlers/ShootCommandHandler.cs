using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Messages.Commands;
using Bc.Battleships.Messages.CommandsResults;
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

            var result = game!.Shoot(new Coordinates(request.Column, request.Row));

            return new ShootResult(result == ShootResultEnum.Hit, game.ComputerPlayer.CurrentHitPoints.Value);
        }
    }
}
