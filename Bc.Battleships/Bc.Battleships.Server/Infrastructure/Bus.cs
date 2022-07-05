using Bc.Battleships.Messages;
using MediatR;

namespace Bc.Battleships.Server.Infrastructure
{
    public class Bus : IBus
    {
        private readonly IMediator _mediator;

        public Bus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Publish(IEvent @event)
        {
            await _mediator.Publish(@event);
        }
    }
}
