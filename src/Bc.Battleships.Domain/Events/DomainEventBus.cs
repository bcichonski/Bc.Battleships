
namespace Bc.Battleships.Domain.Events;

/// <summary>
/// Publishes domain events through the <see cref="IBus"/> of the current scope.
/// Inject <see cref="IDomainEventBus"/> and await <see cref="Publish"/>; there is deliberately no static accessor.
/// </summary>
public class DomainEventBus(IBus bus) : IDomainEventBus
{
    public Task Publish(IDomainEvent domainEvent)
        => bus.Publish(domainEvent);
}
