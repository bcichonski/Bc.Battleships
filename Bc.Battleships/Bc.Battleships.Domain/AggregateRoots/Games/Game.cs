using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.Services;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Domain.ValueObjects.GameContext;

namespace Bc.Battleships.Domain.AggregateRoots.Games
{
    public class Game
    {
        public GameId Id { get; }
        public ComputerPlayer ComputerPlayer { get; }
        public HumanPlayer HumanPlayer { get; } = new HumanPlayer();
        public GameContext GameContext { get; } = GameContextProvider.GameContext;

        public GameState GameState { get; }

        public Game() : this(new ComputerPlayer())
        {
        }

        internal Game(ComputerPlayer computerPlayer)
        {
            Id = new GameId();

            ComputerPlayer = computerPlayer;

            GameState = GameState.Active;
        }

        public void PositionFleets()
        {
            ComputerPlayer.PositionShips(GameContext);
        }

        public ShootResult Shoot(Coordinates coordinates)
            => ComputerPlayer.BraceForImpact(coordinates);
    }
}
