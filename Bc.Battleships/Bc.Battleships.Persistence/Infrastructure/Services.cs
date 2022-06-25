using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Bc.Battleships.Persistence.Infrastructure
{
    public static class Services
    {
        public static void Configure(ServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContextFactory<BattleshipsDbContext>(lifetime: ServiceLifetime.Scoped);
            serviceCollection.AddScoped<IGamesRepository, GamesRepository>();
        }
    }
}
