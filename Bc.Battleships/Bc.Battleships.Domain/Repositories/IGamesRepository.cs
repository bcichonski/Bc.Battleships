using Bc.Battleships.Domain.AggregateRoots.Games;

namespace Bc.Battleships.Domain.Repositories
{
    public interface IGamesRepository
    {
        Task<Game?> Get();

        void Add(Game game);
    }
}
