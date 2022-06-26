namespace Bc.Battleships.Domain.Events
{
    public interface IDomainEventBus
    {
        Task Publish(IDomainEvent domainEvent);
    }
}
