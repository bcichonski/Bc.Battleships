using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Bc.Battleships.Infrastructure.Repositories;

public class GamesRepository(BattleshipsDbContext battleshipsDbContext) : IGamesRepository
{
    public Task<Game?> GetById(GameId gameId, CancellationToken cancellationToken)
        => battleshipsDbContext
            .Set<Game>()
            .FirstOrDefaultAsync(x => x.Id == gameId, cancellationToken);

    public void Add(Game game)
        => battleshipsDbContext.Add(game);
}
