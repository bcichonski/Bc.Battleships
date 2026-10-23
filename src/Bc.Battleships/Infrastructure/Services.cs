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
            serviceCollection.AddSingleton<IPersistenceConfigurationProvider, ConfigurationProvider>();
            PersistenceServices.Configure(serviceCollection);
            HandlersServices.Configure(serviceCollection);

            serviceCollection.AddOpenApi(options =>
                options.AddDocumentTransformer((document, _, _) =>
                {
                    document.Info.Title = "Battleships API";
                    return Task.CompletedTask;
                }));
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
