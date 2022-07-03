using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Messages.Commands;
using MediatR;

namespace Bc.Battleships.Server.CommandHandlers
{
    public class CreateNewGameCommandHandler : AsyncRequestHandler<CreateNewGameCommand>
    {
        private readonly IGamesRepository _gamesRepository;

        public CreateNewGameCommandHandler(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }

        protected override async Task Handle(CreateNewGameCommand request, CancellationToken cancellationToken)
        {
            var currentGame = await _gamesRepository.Get(cancellationToken);
            currentGame!.Stop();

            var newGame = Game.CreateNewGame();
            _gamesRepository.Add(newGame);
        }
    }
}
