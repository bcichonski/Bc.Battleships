using Bc.Battleships.Server.Queries;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using Bc.Battleships.Tests.Common.Builders;
using Bc.Battleships.Domain.AggregateRoots.Games;

namespace Bc.Battleships.Tests.Integration.API.Games
{
    public class WhenRequestingGameState : ApiIntegrationTestBase
    {
        [Fact]
        public async Task ShouldReturnGameState()
        {
            var game = await AssumeGameInDb();
            var gameQuery = new GetCurrentGameQuery();
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            var result = await mediator!.Send(gameQuery, CancellationToken.None);

            result.CurrentHitPoints.Should().BeGreaterThan(0);
            result.Board.BoardSize.Should().Be(game.GameContext.BoardSize.Value);
        }

        private async Task<Game> AssumeGameInDb()
        {
            Game? game = null;
            await InScope(async dbContext =>
            {
                await dbContext!.AddAsync(game = new GameBuilder().Build());

                await dbContext.SaveChangesAsync();
            });
            return game!;
        }
    }
}
