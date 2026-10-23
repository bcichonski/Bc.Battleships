using Bc.Battleships.Messages;

namespace Bc.Battleships.Domain.Events
{
    /// <summary>
    /// Publishes domain events through the <see cref="IBus"/> of the current scope.
    /// Inject <see cref="IDomainEventBus"/> and await <see cref="Publish"/>; there is deliberately no static accessor.
    /// </summary>
    public class DomainEventBus : IDomainEventBus
    {
        private readonly IBus _bus;

        public DomainEventBus(IBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public Task Publish(IDomainEvent domainEvent)
            => _bus.Publish(domainEvent);
    }
}
