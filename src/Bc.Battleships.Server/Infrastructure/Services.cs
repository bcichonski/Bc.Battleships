using Bc.Battleships.Domain.Events;
using Bc.Battleships.Messages;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Bc.Battleships.Server.Infrastructure
{
    public static class Services
    {
        public static void Configure(IServiceCollection serviceCollection)
        {
            // The source generator runs in this project. It must also scan the Domain assembly,
            // where AbandonTheShipDomainEventHandler lives, and the Messages assembly, where the messages live.
            serviceCollection.AddMediator((MediatorOptions options) =>
            {
                options.ServiceLifetime = ServiceLifetime.Scoped;
                options.Assemblies = [typeof(Services), typeof(IDomainEvent), typeof(IEvent)];
            });

            serviceCollection.AddScoped<IBus, Bus>();
        }
    }
}
