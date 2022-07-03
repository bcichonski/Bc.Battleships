using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.ValueObjects;

namespace Bc.Battleships.Domain.Repositories
{
    public interface IGamesRepository
    {
        Task<Game?> Get(CancellationToken cancellationToken);
        Task<Game?> GetById(GameId gameId, CancellationToken cancellationToken);

        Task Add(Game game);
    }
}
