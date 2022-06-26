using FluentAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain
{
    public class WhenCreatingGame
    {
        [Fact]
        public void GameShouldHaveAnId()
        {
            var game = new Game();

            game.Id.Value.Should().NotBeEmpty();
        }

        [Fact]
        public void GameShouldHaveComputerPlayer()
        {
            var game = new Game();

            game.ComputerPlayer.Should().NotBeNull();
        }

        [Fact]
        public void GameShouldHaveHumanPlayer()
        {
            var game = new Game();

            game.HumanPlayer.Should().NotBeNull();
        }
    }
}