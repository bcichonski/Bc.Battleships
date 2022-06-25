using FluentAssertions;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using System.Linq;
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
        public void GameShouldHaveTwoPlayers()
        {
            var game = new Game();

            game.Players.Should().HaveCount(2);
        }

        [Fact]
        public void GameShouldHaveComputerPlayer()
        {
            var game = new Game();

            game.Players.FirstOrDefault(x => x is ComputerPlayer).Should().NotBeNull();
        }

        [Fact]
        public void GameShouldHaveHumanPlayer()
        {
            var game = new Game();

            game.Players.FirstOrDefault(x => x is HumanPlayer).Should().NotBeNull();
        }
    }
}