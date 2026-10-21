using FluentAssertions;
using Bc.Battleships.Messages.Commands;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.API.Games
{
    public class WhenCreatingNewGame : ApiIntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateOne()
        {
            var game = await AssumeGameInDb();

            await InScope(async testContext =>
            {
                await testContext.Mediator.Send(new CreateNewGameCommand(), CancellationToken.None);
            });

            await InScope(async testContext =>
            {
                var oldGame = await testContext.GamesRepository.GetById(game.Id, CancellationToken.None);
                var newGame = await testContext.GamesRepository.Get(CancellationToken.None);

                oldGame.Should().NotBeNull();
                oldGame!.Id.Should().NotBe(newGame!.Id);
                oldGame.GameState.Should().Be(Domain.ValueObjects.Enums.GameState.Ended);

                newGame!.GameState.Should().NotBe(Domain.ValueObjects.Enums.GameState.Ended);
            });
        }
    }
}
