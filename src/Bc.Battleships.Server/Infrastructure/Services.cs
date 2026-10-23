using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Bc.Battleships.Server.Infrastructure
{
    public static class Services
    {
        public static void Configure(IServiceCollection serviceCollection)
        {
            // The source generator runs in this project. It scans this assembly, where the handlers live,
            // and the Messages assembly, where the commands and queries live.
            serviceCollection.AddMediator((MediatorOptions options) =>
            {
                options.ServiceLifetime = ServiceLifetime.Scoped;
                options.Assemblies = [typeof(Services), typeof(Bc.Battleships.Messages.Commands.ShootCommand)];
            });
        }
    }
}
