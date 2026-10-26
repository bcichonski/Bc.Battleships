using AwesomeAssertions;
using Bc.Battleships.Domain.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Events;

public class WhenPublishingDomainEvent
{
    [Fact]
    public async Task ShouldForwardItToTheBus()
    {
        var bus = new RecordingBus();
        var domainEvent = new TestDomainEvent();

        await new DomainEventBus(bus).Publish(domainEvent);

        bus.Published.Should().ContainSingle().Which.Should().BeSameAs(domainEvent);
    }

    private sealed class TestDomainEvent : IDomainEvent
    {
    }

    private sealed class RecordingBus : IBus
    {
        public List<IEvent> Published { get; } = [];

        public Task Publish(IEvent @event)
        {
            Published.Add(@event);
            return Task.CompletedTask;
        }
    }
}
