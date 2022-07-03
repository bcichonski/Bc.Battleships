using Bc.Battleships.Server.Queries;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using Bc.Battleships.Tests.Common.Builders;

namespace Bc.Battleships.Tests.Integration.API.Games
{
    public class WhenRequestingGameState : ApiIntegrationTestBase
    {
        [Fact]
        public async Task ShouldReturnGameState()
        {
            await AssumeGameInDb();
            var gameQuery = new GetCurrentGameQuery();
            var mediator = ServiceProvider.GetRequiredService<IMediator>();


            var result = await mediator!.Send(gameQuery, CancellationToken.None);

            result.Should().NotBeNull();
        }

        private Task AssumeGameInDb()
            => InScope(async dbContext =>
            {
                await dbContext!.AddAsync(new GameBuilder().Build());

                await dbContext.SaveChangesAsync();
            });
    }
}
