using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Infrastructure.Providers;
using Bc.Battleships.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bc.Battleships.Infrastructure.Infrastructure;

public static class Services
{
    public static void Configure(IServiceCollection serviceCollection)
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
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddScoped<DatabaseMigrator>();
    }
}
