using Bc.Battleships.Persistence;

namespace Bc.Battleships.Infrastructure
{
    public static class Services
    {
        public static void Configure(ServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContextFactory<BattleshipsDbContext>();
        }
    }
}
