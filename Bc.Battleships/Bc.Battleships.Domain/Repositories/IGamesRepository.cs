using Bc.Battleships.Domain.AggregateRoots.Games;

namespace Bc.Battleships.Domain.Repositories
{
    public interface IGamesRepository
    {
        Task<Game?> Get(CancellationToken cancellationToken);

        void Add(Game game);
    }
}
