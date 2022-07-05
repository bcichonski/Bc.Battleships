using PersistenceServices = Bc.Battleships.Persistence.Infrastructure.Services;
using HandlersServices = Bc.Battleships.Server.Infrastructure.Services;
using Bc.Battleships.Persistence.Providers;
using Bc.Battleships.Domain.Events;
using Bc.Battleships.Messages;
using Bc.Battleships.Persistence.Infrastructure;

namespace Bc.Battleships.Infrastructure
{
    public static class Services
    {
        public static void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPersistenceConfigurationProvider, ConfigurationProvider>();
            PersistenceServices.Configure(serviceCollection);
            HandlersServices.Configure(serviceCollection);

            serviceCollection.AddControllersWithViews();
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Battleships API"
                });
            });
        }

        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateAsyncScope())
            {
                await scope.ServiceProvider.GetRequiredService<DatabaseMigrator>().ApplyMigrationsIfNeeded();
            }
        }
    }
}
