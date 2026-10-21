using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Messages.Commands;
using Mediator;

namespace Bc.Battleships.Server.CommandHandlers
{
    public class CreateNewGameCommandHandler : IRequestHandler<CreateNewGameCommand>
    {
        private readonly IGamesRepository _gamesRepository;

        public CreateNewGameCommandHandler(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }

        public async ValueTask<Unit> Handle(CreateNewGameCommand request, CancellationToken cancellationToken)
        {
            var currentGame = await _gamesRepository.Get(cancellationToken);
            currentGame?.Stop();

            var newGame = Game.CreateNewGame();
            await _gamesRepository.Add(newGame);

            return Unit.Value;
        }
    }
}
