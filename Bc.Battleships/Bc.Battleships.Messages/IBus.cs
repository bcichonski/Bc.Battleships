namespace Bc.Battleships.Messages
{
    public interface IBus
    {
        Task Publish(IEvent @event);
    }
}
