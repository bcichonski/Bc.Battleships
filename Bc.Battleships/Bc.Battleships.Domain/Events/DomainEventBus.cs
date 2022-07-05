using Bc.Battleships.Messages;

namespace Bc.Battleships.Domain.Events
{
    public class DomainEventBus : IDomainEventBus
    {
        public static Func<IBus>? GetBus { get; set; }
        public static DomainEventBus Instance => new(GetBus!());

        private readonly IBus? _bus;
       
        public DomainEventBus(IBus bus)
        {
            _bus = bus;
        }

        public Task Publish(IDomainEvent domainEvent)
            => _bus!.Publish(domainEvent);
    }
}
