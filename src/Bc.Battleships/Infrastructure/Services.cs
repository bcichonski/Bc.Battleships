using Bc.Battleships.Domain.Events;
using Bc.Battleships.Persistence.Infrastructure;
using Bc.Battleships.Persistence.Providers;
using HandlersServices = Bc.Battleships.Server.Infrastructure.Services;
using PersistenceServices = Bc.Battleships.Persistence.Infrastructure.Services;

namespace Bc.Battleships.Infrastructure
{
    public static class Services
    {
        public static void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpContextAccessor();
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
            DomainEventBus.SetHttpContextAccessor(serviceProvider.GetRequiredService<IHttpContextAccessor>());

            using (var scope = serviceProvider.CreateAsyncScope())
            {
                await scope.ServiceProvider.GetRequiredService<DatabaseMigrator>().ApplyMigrationsIfNeeded();
            }
        }
    }
}
