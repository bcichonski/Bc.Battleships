using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.Enums;
using Bc.Battleships.Domain.ValueObjects;

namespace Bc.Battleships.Domain.AggregateRoots.Games
{
    public class Game
    {
        public GameId Id { get; }
        public IList<Player> Players { get; }

        public GameState GameState { get; }

        public Game()
        {
            Id = new GameId();
            Players = new List<Player>();
            Players.Add(new HumanPlayer());
            Players.Add(new ComputerPlayer());

            GameState = GameState.Active;
        }
    }
}
