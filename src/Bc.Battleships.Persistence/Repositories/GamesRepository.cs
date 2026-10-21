using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Microsoft.EntityFrameworkCore;

namespace Bc.Battleships.Persistence.Repositories
{
    public class GamesRepository : IGamesRepository
    {
        private readonly BattleshipsDbContext _battleshipsDbContext;

        public GamesRepository(BattleshipsDbContext battleshipsDbContext)
        {
            _battleshipsDbContext = battleshipsDbContext ?? throw new ArgumentNullException(nameof(battleshipsDbContext));
        }

        public Task<Game?> Get(CancellationToken cancellationToken)
            => _battleshipsDbContext
                .Set<Game>()
                .FirstOrDefaultAsync(x => x.GameState == GameState.Active, cancellationToken);

        public Task<Game?> GetById(GameId gameId, CancellationToken cancellationToken)
            => _battleshipsDbContext
                .Set<Game>()
                .FirstOrDefaultAsync(x => x.Id == gameId, cancellationToken);


        public async Task Add(Game game) 
        {
            _battleshipsDbContext.Add(game);

            await _battleshipsDbContext.SaveChangesAsync();
        }
    }
}
