using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Persistence.Repositories;
using Bc.Battleships.Server.DTOs;
using Bc.Battleships.Server.Queries;
using Bc.Battleships.Server.Services;
using MediatR;

namespace Bc.Battleships.Server.QueryHandlers
{
    public class GetCurrentGameQueryHandler : IRequestHandler<GetCurrentGameQuery, GameDto>
    {
        IGamesRepository _gamesRepository;

        public GetCurrentGameQueryHandler(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }

        public async Task<GameDto> Handle(GetCurrentGameQuery request, CancellationToken cancellationToken)
        {
            var game = await _gamesRepository.Get(cancellationToken);
            return GameToGameDtoConverter.Convert(game);
        }
    }
}
