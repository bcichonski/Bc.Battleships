using FluentAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Games
{
    public class WhenCreatingGame
    {
        [Fact]
        public void ItShouldHaveAnId()
        {
            var game = new Game();

            game.Id.Value.Should().NotBeEmpty();
        }

        [Fact]
        public void ItShouldHaveComputerPlayer()
        {
            var game = new Game();

            game.ComputerPlayer.Should().NotBeNull();
        }

        [Fact]
        public void ItShouldHaveHumanPlayer()
        {
            var game = new Game();

            game.HumanPlayer.Should().NotBeNull();
        }

        [Fact]
        public void FleetShouldBePositioned()
        {
            var game = new Game();

            game.PositionFleets();

            game.ComputerPlayer.Ships.Should().AllSatisfy(x => x.Compartments.Should().NotBeNull());
        }
    }
}