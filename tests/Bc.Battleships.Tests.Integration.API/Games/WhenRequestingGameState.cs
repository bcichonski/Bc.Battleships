using AwesomeAssertions;
using Bc.Battleships.Application.Queries;
using Bc.Battleships.Domain.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.API.Games;

public class WhenRequestingGameState : ApiIntegrationTestBase
{
    [Fact]
    public async Task ShouldReturnGameState()
    {
        var game = await AssumeGameInDb();

        await InScope(async testContext =>
        {
            var result = await testContext.Mediator.Send(new GetGameQuery(game.Id), CancellationToken.None);

            result.Game.Should().NotBeNull();
            var dto = result.Game!;
            dto.Id.Should().Be(game.Id.Value);
            dto.IsGameActive.Should().BeTrue();
            dto.CurrentHitPoints.Should().BeGreaterThan(0);
            dto.Board.BoardSize.Should().Be(10);
        });
    }

    [Fact]
    public async Task ShouldReturnNoGameForUnknownId()
    {
        await AssumeGameInDb();

        await InScope(async testContext =>
        {
            var result = await testContext.Mediator.Send(new GetGameQuery(new GameId(Guid.NewGuid())), CancellationToken.None);

            result.Game.Should().BeNull();
        });
    }

    [Fact]
    public async Task ShouldReturnInactiveGameWhenGameIsStopped()
    {
        var game = await AssumeGameInDb();
        await InScope(async testContext =>
        {
            var stored = await testContext.GamesRepository.GetById(game.Id, CancellationToken.None);
            stored!.Stop();
            await testContext.DbContext.SaveChangesAsync();
        });

        await InScope(async testContext =>
        {
            var result = await testContext.Mediator.Send(new GetGameQuery(game.Id), CancellationToken.None);

            result.Game!.IsGameActive.Should().BeFalse();
        });
    }
}
