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

        public GameState GameState { get; private set; }

#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal Game(GameId id, ComputerPlayer? computerPlayer)
        {
            Id = id;

            ComputerPlayer = computerPlayer;
            GameState = GameState.Active;
        }

        private Game()

        {
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning restore CS8601 // Possible null reference assignment.

        public void PositionFleets()
        {
            ComputerPlayer.PositionShips(GameContext);
        }

        public ShootResult Shoot(Coordinates coordinates)
        {
            var result = ComputerPlayer.IncomingShot(coordinates);

            if (GameState == GameState.Active && ComputerPlayer.CurrentHitPoints.Value == 0)
            {
                GameState = GameState.Won;
            }

            return result;
        }

        public void Stop()
        {
            GameState = GameState.Ended;
        }

        public static Game CreateNewGame()
        {
            var newGame = new Game(new GameId(), new ComputerPlayer());

            newGame.PositionFleets();

            return newGame;
        }
    }
}
