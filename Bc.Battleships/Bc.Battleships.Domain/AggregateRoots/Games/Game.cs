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

        public Game(GameId id, ComputerPlayer computerPlayer)
        {
            Id = id;

            ComputerPlayer = computerPlayer;

            GameState = GameState.Active;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Game()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public void PositionFleets()
        {
            ComputerPlayer.PositionShips();
        }

        public ShootResult Shoot(Coordinates coordinates)
            => ComputerPlayer.IncomingShot(coordinates);
    }
}
