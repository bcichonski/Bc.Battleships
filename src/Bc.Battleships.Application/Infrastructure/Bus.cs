using Bc.Battleships.Domain.Events;
using Mediator;

namespace Bc.Battleships.Application.Infrastructure;

public class Bus(IMediator mediator) : IBus
{
    public async Task Publish(IEvent @event)
    {
        await mediator.Publish(@event);
    }
}
