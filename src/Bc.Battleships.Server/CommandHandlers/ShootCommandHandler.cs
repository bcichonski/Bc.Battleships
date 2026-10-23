using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Messages.Commands;
using Bc.Battleships.Messages.CommandsResults;
using Bc.Battleships.Persistence;
using Mediator;
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

        public async ValueTask<ShootResult> Handle(ShootCommand request, CancellationToken cancellationToken)
        {
            var game = await _gamesRepository.Get(cancellationToken);

            if (game == null)
            {
                return new ShootResult(false, GAME_ENDED);
            }

            var coordinates = new Coordinates(request.Column + 1, request.Row + 1);
            var result = game!.Shoot(coordinates);

            await _battleshipsDbContext.SaveChangesAsync(cancellationToken);

            return new ShootResult(IsCellHit(game, coordinates, result), game.ComputerPlayer.CurrentHitPoints.Value);
        }

        private static bool IsCellHit(Game game, Coordinates coordinates, ShootResultEnum result)
            => result switch
            {
                ShootResultEnum.Hit or ShootResultEnum.Sunk => true,
                ShootResultEnum.AlreadyShot => game.ComputerPlayer.IsFloodedAt(coordinates),
                _ => false
            };
    }
}
