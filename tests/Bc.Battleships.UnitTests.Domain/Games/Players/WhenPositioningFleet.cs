using AwesomeAssertions;
using System.Linq;
using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Tests.Common.Builders;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Games.Players;

public class WhenPositioningFleet
{
    [Fact]
    public void AllShipsShouldBePositioned()
    {
        var game = new GameBuilder()
            .WithNotPositionedFleet()
            .Build();
        var player = game.ComputerPlayer;

        player.PositionShips(GameContextBuilder.Standard);

        player.Ships.Should().NotBeNull();
        player.Ships.Should().AllSatisfy(x => x.Compartments.Should().NotBeNull());
    }

    [Fact]
    public void FleetShouldHonourNonDefaultContext()
    {
        var context = GameContextBuilder.Create(5, 2, 3);
        var player = new ComputerPlayer();

        player.PositionShips(context);

        player.Ships.Should().HaveCount(2);
        player.Ships!.Select(x => x.Length.Value).Should().BeEquivalentTo(new[] { 2, 3 });
        player.Ships!.SelectMany(x => x.Compartments).Should().AllSatisfy(c =>
        {
            c.Coordinates.Column.Should().BeInRange(1, 5);
            c.Coordinates.Row.Should().BeInRange(1, 5);
        });
    }
}
