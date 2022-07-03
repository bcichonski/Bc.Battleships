using FluentAssertions;
using Bc.Battleships.Messages.Queries;
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

            await InScope(async testContext =>
            {
                var result = await testContext.Mediator.Send(gameQuery, CancellationToken.None);

                result.CurrentHitPoints.Should().BeGreaterThan(0);
                result.Board.BoardSize.Should().Be(game.GameContext.BoardSize.Value);
            });
        }
    }
}
