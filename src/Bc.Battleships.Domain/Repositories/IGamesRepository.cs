using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.ValueObjects;

namespace Bc.Battleships.Domain.Repositories;

public interface IGamesRepository
{
    Task<Game?> GetById(GameId gameId, CancellationToken cancellationToken);

    /// <summary>
    /// Tracks a new game. Nothing is persisted until <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
    /// </summary>
    void Add(Game game);
}
