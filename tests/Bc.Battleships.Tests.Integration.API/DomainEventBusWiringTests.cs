using AwesomeAssertions;
using Bc.Battleships.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.API;

public class DomainEventBusWiringTests : ApiIntegrationTestBase
{
    [Fact]
    public async Task ShouldResolveScopedDomainEventBusAndPublishThroughMediator()
    {
        await InScope(async testContext =>
        {
            var domainEventBus = testContext.Services.GetRequiredService<IDomainEventBus>();

            Func<Task> publish = () => domainEventBus.Publish(new UnhandledDomainEvent());

            await publish.Should().NotThrowAsync();
        });
    }

    private sealed class UnhandledDomainEvent : IDomainEvent
    {
    }
}
