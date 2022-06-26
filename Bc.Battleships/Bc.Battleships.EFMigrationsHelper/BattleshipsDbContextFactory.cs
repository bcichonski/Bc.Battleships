using Bc.Battleships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bc.Battleships.EFMigrationsHelper
{
    public class BattleshipsDbContextFactory : IDesignTimeDbContextFactory<BattleshipsDbContext>
    {
        public BattleshipsDbContext CreateDbContext(string[] args)
        {
            return new BattleshipsDbContext(new DbContextOptionsBuilder()
                .UseSqlite(new DesignTimePersistenceConfigurationProvider().ConnectionString)
                    .Options);
        }
    }
}
