using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Messages.DTOs;
using Bc.Battleships.Messages.Queries;
using Bc.Battleships.Server.Services;
using Mediator;

namespace Bc.Battleships.Server.QueryHandlers
{
    public class GetCurrentGameQueryHandler : IRequestHandler<GetCurrentGameQuery, GameDto>
    {
        private readonly IGamesRepository _gamesRepository;

        public GetCurrentGameQueryHandler(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }

        public async ValueTask<GameDto> Handle(GetCurrentGameQuery request, CancellationToken cancellationToken)
        {
            var game = await _gamesRepository.Get(cancellationToken);
            
            if (game == null)
            {
                return new GameDto(false, null, 0);
            }

            return GameToGameDtoConverter.Convert(game);
        }
    }
}
