using Bc.Battleships.Persistence.Providers;
using Microsoft.EntityFrameworkCore;

namespace Bc.Battleships.Persistence
{
    public class BattleshipsDbContext : DbContext
    {
        private readonly string _connectionString;

        public BattleshipsDbContext(IPersistenceConfigurationProvider persistenceConfigurationProvider) : base()
        {
            _connectionString = persistenceConfigurationProvider.ConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
             
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}