using Bc.Battleships.Application.Options;
using Bc.Battleships.Infrastructure.Infrastructure;
using Bc.Battleships.Infrastructure.Providers;
using HandlersServices = Bc.Battleships.Application.Infrastructure.Services;
using PersistenceServices = Bc.Battleships.Infrastructure.Infrastructure.Services;

namespace Bc.Battleships.Infrastructure;

public static class Services
{
    public static void Configure(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPersistenceConfigurationProvider, ConfigurationProvider>();
        PersistenceServices.Configure(serviceCollection);
        HandlersServices.Configure(serviceCollection);

        serviceCollection.AddOptions<GameOptions>()
            .BindConfiguration(GameOptions.SectionName)
            .ValidateOnStart();

        // Liveness only: no dependency checks, so the container probe reflects process health.
        serviceCollection.AddHealthChecks();

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
