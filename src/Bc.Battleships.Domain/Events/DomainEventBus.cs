using Bc.Battleships.Messages;
using Microsoft.AspNetCore.Http;

namespace Bc.Battleships.Domain.Events
{
    public class DomainEventBus : IDomainEventBus
    {
        private static IHttpContextAccessor? _httpContextAccessor;
        private static IDomainEventBus? _domainEventBus;
        
        public static IDomainEventBus Instance => _domainEventBus ?? new DomainEventBus((IBus) _httpContextAccessor!.HttpContext.RequestServices.GetService(typeof(IBus))!);

        private readonly IBus? _bus;

        public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
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
