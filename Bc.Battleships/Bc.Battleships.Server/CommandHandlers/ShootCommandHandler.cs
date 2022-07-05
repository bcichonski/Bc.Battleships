using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Messages.Commands;
using Bc.Battleships.Messages.CommandsResults;
using Bc.Battleships.Persistence;
using MediatR;
using ShootResultEnum = Bc.Battleships.Domain.ValueObjects.Enums.ShootResult;

namespace Bc.Battleships.Server.CommandHandlers
{
    internal class ShootCommandHandler : IRequestHandler<ShootCommand, ShootResult>
    {
        private const int GAME_ENDED = -1;

        private readonly IGamesRepository _gamesRepository;
        private readonly BattleshipsDbContext _battleshipsDbContext;

        public ShootCommandHandler(IGamesRepository gamesRepository, BattleshipsDbContext battleshipsDbContext)
        {
            _gamesRepository = gamesRepository;
            _battleshipsDbContext = battleshipsDbContext;
        }

        public async Task<ShootResult> Handle(ShootCommand request, CancellationToken cancellationToken)
        {
            var game = await _gamesRepository.Get(cancellationToken);

            if (game == null)
            {
                return new ShootResult(false, GAME_ENDED);
            }

            var result = game!.Shoot(new Coordinates(request.Column + 1, request.Row + 1));

            await _battleshipsDbContext.SaveChangesAsync(cancellationToken);

            return new ShootResult(result == ShootResultEnum.Hit, game.ComputerPlayer.CurrentHitPoints.Value);
        }
    }
}
