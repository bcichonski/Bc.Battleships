using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bc.Battleships.Infrastructure;

public class BattleshipsDbContextFactory : IDesignTimeDbContextFactory<BattleshipsDbContext>
{
    public BattleshipsDbContext CreateDbContext(string[] args)
    {
        return new BattleshipsDbContext(new DbContextOptionsBuilder()
            .UseSqlite(new DesignTimePersistenceConfigurationProvider().ConnectionString)
                .Options);
    }
}
