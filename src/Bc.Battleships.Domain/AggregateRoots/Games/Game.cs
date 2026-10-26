using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Domain.ValueObjects.GameContext;

namespace Bc.Battleships.Domain.AggregateRoots.Games;

public class Game
{
    public GameId Id { get; }
    public ComputerPlayer ComputerPlayer { get; }
    public HumanPlayer HumanPlayer { get; } = new HumanPlayer();

    public GameState GameState { get; private set; }

    internal Game(GameId id, ComputerPlayer computerPlayer)
    {
        Id = id;

        ComputerPlayer = computerPlayer;
        GameState = GameState.Active;
    }

    private Game()
    {
        ComputerPlayer = null!;
    }

    public void PositionFleets(GameContext gameContext, Random? random = null)
    {
        ComputerPlayer.PositionShips(gameContext, random);
    }

    public ShootResult Shoot(Coordinates coordinates)
    {
        var result = ComputerPlayer.IncomingShot(coordinates);

        if (result == ShootResult.AlreadyShot)
        {
            return result;
        }

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

    public static Game CreateNewGame(GameContext gameContext, Random? random = null)
    {
        var newGame = new Game(GameId.New(), new ComputerPlayer());

        newGame.PositionFleets(gameContext, random);

        return newGame;
    }
}
