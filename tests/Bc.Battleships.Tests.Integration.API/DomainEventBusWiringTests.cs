using AwesomeAssertions;
using Bc.Battleships.Domain.Events;
using Bc.Battleships.Tests.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.API;

public class DomainEventBusWiringTests
{
    [Fact]
    public async Task ShouldResolveScopedDomainEventBusFromTheHostAndPublishWithoutHandlers()
    {
        using var factory = new BattleshipsWebApplicationFactory();
        using var scope = factory.Services.CreateScope();
        var domainEventBus = scope.ServiceProvider.GetRequiredService<IDomainEventBus>();

        Func<Task> publish = () => domainEventBus.Publish(new UnhandledDomainEvent());

        await publish.Should().NotThrowAsync();
    }

    private sealed class UnhandledDomainEvent : IDomainEvent
    {
    }
}
