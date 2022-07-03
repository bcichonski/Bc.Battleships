using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Bc.Battleships.Persistence.Infrastructure
{
    public class DatabaseMigrator
    {
        private readonly BattleshipsDbContext _dbContext;

        public DatabaseMigrator(BattleshipsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ApplyMigrationsIfNeeded()
        {
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

            if (!pendingMigrations.Any())
            {
                return;
            }
            await _dbContext.Database.MigrateAsync();
        }
    }
}
