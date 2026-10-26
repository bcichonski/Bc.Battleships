using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Application.DTOs;

namespace Bc.Battleships.Application.Services;

internal static class GameToGameDtoConverter
{
    public static GameDto Convert(Game game, int boardSize)
        => new(
            game.Id.Value,
            game.GameState == GameState.Active,
            ConvertBoard(game, boardSize),
            game.ComputerPlayer.CurrentHitPoints.Value);

    private static BoardDto ConvertBoard(Game game, int boardSize)
        => new(GetBoardState(boardSize, game.ComputerPlayer));

    private static BoardRowDto[] GetBoardState(int boardSize, ComputerPlayer computerPlayer)
    {
        ArgumentNullException.ThrowIfNull(computerPlayer.Ships);

        var boardShoot = new HashSet<Coordinates>(computerPlayer.CoordinatesShoot.Select(x => x.Coordinates));

        BoardRowDto[] boardRows = CreateBoard(boardSize);
        PutShots(boardShoot, boardRows);
        PlaceShips(computerPlayer, boardShoot, boardRows);

        return boardRows;
    }

    private static void PlaceShips(ComputerPlayer computerPlayer, HashSet<Coordinates> boardShoot, BoardRowDto[] boardRows)
    {
        foreach (var ship in computerPlayer.Ships!)
        {
            Place(ship, boardRows, boardShoot);
        }
    }

    private static void PutShots(HashSet<Coordinates> boardShoot, BoardRowDto[] boardRows)
    {
        foreach (var shot in boardShoot)
        {
            boardRows[shot.Row - 1].Row[shot.Column - 1] = BoardFieldState.Miss;
        }
    }

    private static BoardRowDto[] CreateBoard(int boardSize)
    {
        var boardRows = new BoardRowDto[boardSize];
        for (int row = 0; row < boardSize; row++)
        {
            boardRows[row] = new BoardRowDto(new BoardFieldState[boardSize]);
        }

        return boardRows;
    }

    private static void Place(Ship ship, BoardRowDto[] boardRows, HashSet<Coordinates> boardShoot)
    {
        // Each compartment is placed at its own coordinates: the order of an owned collection loaded from the
        // database is not guaranteed, so positional indexing would misplace flooded cells after a reload.
        foreach (var compartment in ship.Compartments)
        {
            boardRows[compartment.Coordinates.Row - 1].Row[compartment.Coordinates.Column - 1] = ToBoardState(compartment, boardShoot);
        }
    }

    private static BoardFieldState ToBoardState(ShipCompartment shipCompartment, HashSet<Coordinates> boardShoot)
    {
        if (shipCompartment.State == ShipCompartmentState.Flooded)
        {
            return BoardFieldState.Hit;
        }

        if (boardShoot.Contains(shipCompartment.Coordinates))
        {
            return BoardFieldState.Miss;
        }

        return BoardFieldState.Unknown;
    }
}
