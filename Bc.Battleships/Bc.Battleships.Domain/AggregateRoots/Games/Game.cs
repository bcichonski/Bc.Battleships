using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;

namespace Bc.Battleships.Domain.AggregateRoots.Games
{
    public class Game
    {
        public GameId Id { get; }
        public ComputerPlayer ComputerPlayer { get; }
        public HumanPlayer HumanPlayer { get; } = new HumanPlayer();

        public GameState GameState { get; }

        public Game()
        {
            Id = new GameId();
            
            ComputerPlayer = new ComputerPlayer();

            GameState = GameState.Active;
        }
    }
}
