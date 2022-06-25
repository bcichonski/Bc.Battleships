using Bc.Battleships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bc.Battleships.EFMigrationsHelper
{
    public class BattleshipsDbContextFactory : IDesignTimeDbContextFactory<BattleshipsDbContext>
    {
        public BattleshipsDbContext CreateDbContext(string[] args)
        {
            //var optionsBuilder = new DbContextOptionsBuilder<BattleshipsDbContext>();
            //optionsBuilder.UseSqlite("Data Source=blog.db");

            return new BattleshipsDbContext(new DesignTimePersistenceConfigurationProvider());
        }
    }
}
