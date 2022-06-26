using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.Enums;
using Bc.Battleships.Domain.ValueObjects;

namespace Bc.Battleships.Domain.AggregateRoots.Games
{
    public class Game
    {
        public GameId Id { get; }
        public ComputerPlayer ComputerPlayer { get; }
        public HumanPlayer HumanPlayer { get; }

        public GameState GameState { get; }

        public Game()
        {
            Id = new GameId();
            
            ComputerPlayer = new ComputerPlayer();
            HumanPlayer = new HumanPlayer();

            GameState = GameState.Active;
        }
    }
}
