using AwesomeAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Infrastructure.Repositories;
using Bc.Battleships.Tests.Common.Builders;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bc.Battleships.Tests.Integration.Database.Games;

public class WhenGameIsRoundTripped : IntegrationTestBase
{
    [Fact]
    public async Task ShouldKeepIdsCoordinatesAndLengths()
    {
        var game = Game.CreateNewGame(GameContextBuilder.Standard);

        await InScope(async dbContext =>
        {
            await dbContext.AddAsync(game);
            await dbContext.SaveChangesAsync();
        });

        await InScope(async dbContext =>
        {
            var loaded = await new GamesRepository(dbContext).GetById(game.Id, CancellationToken.None);

            loaded.Should().NotBeNull();
            loaded!.Id.Should().Be(game.Id);
            loaded.ComputerPlayer.Id.Should().Be(game.ComputerPlayer.Id);
            loaded.ComputerPlayer.Ships!.Select(x => x.Id).Should().BeEquivalentTo(game.ComputerPlayer.Ships!.Select(x => x.Id));
            loaded.ComputerPlayer.Ships!.Select(x => x.Length).Should().BeEquivalentTo(game.ComputerPlayer.Ships!.Select(x => x.Length));
            loaded.ComputerPlayer.Ships!.Select(x => x.Coordinates).Should().BeEquivalentTo(game.ComputerPlayer.Ships!.Select(x => x.Coordinates));
            loaded.ComputerPlayer.Ships!.SelectMany(x => x.Compartments).Select(x => x.Id)
                .Should().BeEquivalentTo(game.ComputerPlayer.Ships!.SelectMany(x => x.Compartments).Select(x => x.Id));
        });
    }
}
