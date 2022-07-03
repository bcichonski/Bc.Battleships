using EnsureThat;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Messages.DTOs;

namespace Bc.Battleships.Server.Services
{
    internal static class GameToGameDtoConverter
    {
        public static GameDto Convert(Game game)
            => new(true, ConvertBoard(game), game!.ComputerPlayer.CurrentHitPoints.Value);

        private static BoardDto ConvertBoard(Game? game)
            => new(GetBoardState(game!.GameContext.BoardSize.Value, game.ComputerPlayer));

        private static BoardRowDto[] GetBoardState(int boardSize, ComputerPlayer computerPlayer)
        {
            Ensure.That(computerPlayer.Ships).IsNotNull();

            var boardShoot = new HashSet<Coordinates>(computerPlayer.CoordinatesShoot.Select(x => x.Coordinates));

            var boardRows = new BoardRowDto[boardSize];
            for (int row = 0; row < boardSize; row++)
            {
                boardRows[row] = new BoardRowDto(new BoardFieldState[boardSize]);
            }

            foreach (var ship in computerPlayer.Ships!)
            {
                Place(ship, boardRows, boardShoot);
            }

            return boardRows;
        }

        private static void Place(Ship ship, BoardRowDto[] boardRows, HashSet<Coordinates> boardShoot)
        {
            if (ship.Layout == ShipLayout.Horizontal)
            {
                PlaceHorizontal(ship, boardRows, boardShoot);
                return;
            }

            PlaceVertical(ship, boardRows, boardShoot);
        }

        private static void PlaceVertical(Ship ship, BoardRowDto[] boardRows, HashSet<Coordinates> boardShoot)
        {
            for (int i = 0; i < ship.Length.Value; i++)
            {
                boardRows[ship.Coordinates.Row + i].Row[ship.Coordinates.Column] = ToBoardState(ship.Compartments[i], boardShoot);
            }
        }

        private static void PlaceHorizontal(Ship ship, BoardRowDto[] boardRows, HashSet<Coordinates> boardShoot)
        {
            for (int i = 0; i < ship.Length.Value; i++)
            {
                boardRows[ship.Coordinates.Row].Row[ship.Coordinates.Column + i] = ToBoardState(ship.Compartments[i], boardShoot);
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
}
