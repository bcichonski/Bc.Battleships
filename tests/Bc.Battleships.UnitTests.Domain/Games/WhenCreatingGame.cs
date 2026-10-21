using AwesomeAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Tests.Common.Builders;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Games
{
    public class WhenCreatingGame
    {
        [Fact]
        public void ItShouldHaveAnId()
        {
            var game = new Game(new GameId(), new ComputerPlayerBuilder().Build());

            game.Id.Value.Should().NotBeEmpty();
        }

        [Fact]
        public void ItShouldHaveComputerPlayer()
        {
            var game = new Game(new GameId(), new ComputerPlayerBuilder().Build());

            game.ComputerPlayer.Should().NotBeNull();
        }

        [Fact]
        public void ItShouldHaveHumanPlayer()
        {
            var game = new Game(new GameId(), new ComputerPlayerBuilder().Build());

            game.HumanPlayer.Should().NotBeNull();
        }

        [Fact]
        public void FleetShouldBePositioned()
        {
            var game = new GameBuilder()
                .WithNotPositionedFleet()
                .Build();

            game.PositionFleets();

            game.ComputerPlayer.Ships.Should().AllSatisfy(x => x.Compartments.Should().NotBeNull());
        }
    }
}