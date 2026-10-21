using Microsoft.EntityFrameworkCore;

namespace Bc.Battleships.Persistence
{
    public class BattleshipsDbContext : DbContext
    {
        public BattleshipsDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}