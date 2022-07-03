using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Persistence.Providers;
using Bc.Battleships.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bc.Battleships.Persistence.Infrastructure
{
    public static class Services
    {
        public static void Configure(ServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<BattleshipsDbContext, BattleshipsDbContext>(
                (serviceProvider, dbContextOptionsBuilder) => dbContextOptionsBuilder
                    /*.EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .LogTo(Console.WriteLine)*/
                    .UseSqlite(serviceProvider!
                        .GetService<IPersistenceConfigurationProvider>()!
                        .ConnectionString),
                ServiceLifetime.Scoped,
                optionsLifetime: ServiceLifetime.Singleton);

            serviceCollection.AddScoped<IGamesRepository, GamesRepository>();
        }
    }
}
