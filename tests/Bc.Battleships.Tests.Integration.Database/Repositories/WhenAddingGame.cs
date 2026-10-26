using AwesomeAssertions;
using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Tests.Common.Builders;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.Database.Repositories;

public class WhenAddingGame : IntegrationTestBase
{
    [Fact]
    public async Task ShouldPersistGameWhenUnitOfWorkSaves()
    {
        var game = new GameBuilder().Build();

        await using (var scope = ServiceProvider.CreateAsyncScope())
        {
            scope.ServiceProvider.GetRequiredService<IGamesRepository>().Add(game);
            await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().SaveChangesAsync(CancellationToken.None);
        }

        await using (var scope = ServiceProvider.CreateAsyncScope())
        {
            var stored = await scope.ServiceProvider.GetRequiredService<IGamesRepository>().GetById(game.Id, CancellationToken.None);

            stored.Should().NotBeNull();
            stored!.Id.Should().Be(game.Id);
        }
    }

    [Fact]
    public async Task ShouldNotPersistGameWithoutUnitOfWorkSave()
    {
        var game = new GameBuilder().Build();

        await using (var scope = ServiceProvider.CreateAsyncScope())
        {
            scope.ServiceProvider.GetRequiredService<IGamesRepository>().Add(game);
        }

        await using (var scope = ServiceProvider.CreateAsyncScope())
        {
            var stored = await scope.ServiceProvider.GetRequiredService<IGamesRepository>().GetById(game.Id, CancellationToken.None);

            stored.Should().BeNull();
        }
    }
}
