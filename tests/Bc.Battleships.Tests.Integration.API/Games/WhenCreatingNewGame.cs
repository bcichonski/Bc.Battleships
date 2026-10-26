using AwesomeAssertions;
using Bc.Battleships.Application.Commands;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.API.Games;

public class WhenCreatingNewGame : ApiIntegrationTestBase
{
    [Fact]
    public async Task ShouldCreateAndPersistAnActiveGame()
    {
        Application.DTOs.GameDto? created = null;

        await InScope(async testContext =>
            created = await testContext.Mediator.Send(new CreateNewGameCommand(), CancellationToken.None));

        created.Should().NotBeNull();
        var game = created!;
        game.IsGameActive.Should().BeTrue();
        game.Board.BoardSize.Should().Be(10);

        await InScope(async testContext =>
        {
            var stored = await testContext.GamesRepository.GetById(new GameId(game.Id), CancellationToken.None);

            stored.Should().NotBeNull();
            stored!.GameState.Should().Be(GameState.Active);
        });
    }

    [Fact]
    public async Task ShouldNotChangeAnyOtherGame()
    {
        var existingGame = await AssumeGameInDb();

        await InScope(async testContext =>
            await testContext.Mediator.Send(new CreateNewGameCommand(), CancellationToken.None));

        await InScope(async testContext =>
        {
            var oldGame = await testContext.GamesRepository.GetById(existingGame.Id, CancellationToken.None);

            oldGame.Should().NotBeNull();
            oldGame!.GameState.Should().Be(GameState.Active);
        });
    }
}
