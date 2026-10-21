using Bc.Battleships.Messages;

namespace Bc.Battleships.Domain.Events
{
    public class DomainEventBus : IDomainEventBus
    {
        private static Func<IBus?>? _currentBusResolver;
        private static IDomainEventBus? _domainEventBus;
        
        public static IDomainEventBus Instance => _domainEventBus ?? new DomainEventBus(_currentBusResolver!()!);

        private readonly IBus? _bus;

        /// <summary>Supplies how to resolve the <see cref="IBus"/> of the current request scope (set by the host at startup).</summary>
        public static void SetCurrentBusResolver(Func<IBus?> resolver)
        {
            _currentBusResolver = resolver;
        }

        internal static void Use(IBus bus)
        {
            _domainEventBus = new DomainEventBus(bus);
        }

        public DomainEventBus(IBus bus)
        {
            _bus = bus;
        }

        public Task Publish(IDomainEvent domainEvent)
            => _bus!.Publish(domainEvent);
    }
}
