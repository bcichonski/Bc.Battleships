using Bc.Battleships.Messages;

namespace Bc.Battleships.Domain.Events
{
    public class DomainEventBus : IDomainEventBus
    {
        private static IBus? _bus;
        public static IDomainEventBus Instance { get; } = new DomainEventBus();

        public static void Use(IBus bus)
        {
            _bus = bus;
        }

        public Task Publish(IDomainEvent domainEvent)
            => _bus!.Publish(domainEvent);
    }
}
