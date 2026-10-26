using Bc.Battleships.Application.Options;
using Bc.Battleships.Application.Queries;
using Bc.Battleships.Application.Services;
using Bc.Battleships.Domain.Repositories;
using Mediator;
using Microsoft.Extensions.Options;

namespace Bc.Battleships.Application.QueryHandlers;

public class GetGameQueryHandler(IGamesRepository gamesRepository, IOptions<GameOptions> options)
    : IRequestHandler<GetGameQuery, GetGameQueryResult>
{
    public async ValueTask<GetGameQueryResult> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        var game = await gamesRepository.GetById(request.GameId, cancellationToken);

        return new GetGameQueryResult(
            game is null ? null : GameToGameDtoConverter.Convert(game, options.Value.BoardSize));
    }
}
