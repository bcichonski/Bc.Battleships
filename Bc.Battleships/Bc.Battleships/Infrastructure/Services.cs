using PersistenceServices = Bc.Battleships.Persistence.Infrastructure.Services;
using HandlersServices = Bc.Battleships.Server.Infrastructure.Services;
using Bc.Battleships.Persistence.Providers;

namespace Bc.Battleships.Infrastructure
{
    public static class Services
    {
        public static void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPersistenceConfigurationProvider, ConfigurationProvider>();
            PersistenceServices.Configure(serviceCollection);
            HandlersServices.Configure(serviceCollection);
        }
    }
}
