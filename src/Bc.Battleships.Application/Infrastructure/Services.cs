using Bc.Battleships.Domain.Events;
using Bc.Battleships.Application.Options;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bc.Battleships.Application.Infrastructure;

public static class Services
{
    public static void Configure(IServiceCollection serviceCollection)
    {
        // The source generator runs in this project. It also scans the Domain assembly, so domain event
        // handlers can live next to the domain, and this assembly, where the messages live.
        serviceCollection.AddMediator((MediatorOptions options) =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
            options.Assemblies = [typeof(Services), typeof(IDomainEvent)];
        });

        serviceCollection.AddOptions<GameOptions>();
        serviceCollection.AddSingleton<IValidateOptions<GameOptions>, GameOptionsValidator>();

        serviceCollection.AddScoped<IBus, Bus>();
        serviceCollection.AddScoped<IDomainEventBus, DomainEventBus>();
    }
}
