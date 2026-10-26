using AwesomeAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Tests.Common.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.Database.Repositories;

public class WhenGettingGame : IntegrationTestBase
{
    [Fact]
    public async Task ShouldReturnGame()
    {
        var gameId = await AssumeGameInDb();

        await InScope(async (_) =>
        {
            var gameRepository = ServiceProvider.GetService<IGamesRepository>();
            var game = await gameRepository!.GetById(gameId, CancellationToken.None);

            game!.Should().NotBeNull();
            game!.Id.Value.Should().NotBeEmpty();
        });
    }

    [Fact]
    public async Task ShouldPreservePlayerTypes()
    {
        var gameId = await AssumeGameInDb();

        await InScope(async (_) =>
        {
            var gameRepository = ServiceProvider.GetService<IGamesRepository>();
            var game = await gameRepository!.GetById(gameId, CancellationToken.None);

            game!.ComputerPlayer.Should().NotBeNull();
            game!.HumanPlayer.Should().NotBeNull();
        });
    }

    [Fact]
    public async Task ShouldPreserveFleetState()
    {
        var gameId = await AssumeGameInProgressInDb();

        await InScope(async (_) =>
        {
            var gameRepository = ServiceProvider.GetService<IGamesRepository>();
            var game = await gameRepository!.GetById(gameId, CancellationToken.None);

            game!.ComputerPlayer.CurrentHitPoints.Value.Should().BeGreaterThan(0);
        });
    }

    [Fact]
    public async Task ShouldReturnNullForUnknownId()
    {
        await AssumeGameInDb();

        await InScope(async (_) =>
        {
            var gameRepository = ServiceProvider.GetService<IGamesRepository>();
            var game = await gameRepository!.GetById(new GameId(Guid.NewGuid()), CancellationToken.None);

            game.Should().BeNull();
        });
    }

    private Task<GameId> AssumeGameInProgressInDb()
        => AssumeGameInDb();

    private async Task<GameId> AssumeGameInDb()
    {
        var game = new GameBuilder().Build();

        await InScope(async dbContext =>
            {
                await dbContext!.AddAsync(game);
                await dbContext.SaveChangesAsync();
            });

        return game.Id;
    }
}