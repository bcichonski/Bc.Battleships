using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Bc.Battleships.Infrastructure.Infrastructure;

public class DatabaseMigrator(BattleshipsDbContext dbContext)
{
    public async Task ApplyMigrationsIfNeeded()
    {
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

        if (!pendingMigrations.Any())
        {
            return;
        }
        await dbContext.Database.MigrateAsync();
    }
}
