namespace Bc.Battleships.Domain.Events;

public interface IBus
{
    Task Publish(IEvent @event);
}
