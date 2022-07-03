using FluentAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Messages.Queries;
using Bc.Battleships.Tests.Common.Builders;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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
