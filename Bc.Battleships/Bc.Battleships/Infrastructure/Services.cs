using PersistenceServices = Bc.Battleships.Persistence.Infrastructure.Services;
using HandlersServices = Bc.Battleships.Server.Infrastructure.Services;

namespace Bc.Battleships.Infrastructure
{
    public static class Services
    {
        public static void Configure(IServiceCollection serviceCollection)
        {
            PersistenceServices.Configure(serviceCollection);
            HandlersServices.Configure(serviceCollection);
        }
    }
}
