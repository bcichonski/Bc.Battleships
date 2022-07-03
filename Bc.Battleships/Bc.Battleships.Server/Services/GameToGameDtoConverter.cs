using EnsureThat;
using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Server.DTOs;

namespace Bc.Battleships.Server.Services
{
    internal static class GameToGameDtoConverter
    {
        public static GameDto Convert(Game? game)
            => new GameDto(ConvertBoard(game), game!.ComputerPlayer.CurrentHitPoints.Value);

        private static BoardDto ConvertBoard(Game? game)
            => new BoardDto(GetBoardState(game!.GameContext.BoardSize.Value, game.ComputerPlayer));

        private static BoardFieldState[,] GetBoardState(int boardSize, ComputerPlayer computerPlayer)
        {
            Ensure.That(computerPlayer.Ships).IsNotNull();

            var board = new BoardFieldState[boardSize, boardSize];
            var boardShoot = new HashSet<Coordinates>(computerPlayer.CoordinatesShoot.Select(x => x.Coordinates));

            foreach (var ship in computerPlayer.Ships!)
            {
                Place(ship, board, boardShoot);
            }

            return board;
        }

        private static void Place(Ship ship, BoardFieldState[,] board, HashSet<Coordinates> boardShoot)
        {
            if (ship.Layout == ShipLayout.Horizontal)
            {
                PlaceHorizontal(ship, board, boardShoot);
                return;
            }

            PlaceVertical(ship, board, boardShoot);
        }

        private static void PlaceVertical(Ship ship, BoardFieldState[,] board, HashSet<Coordinates> boardShoot)
        {
            for (int i = 0; i < ship.Length.Value; i++)
            {
                board[ship.Coordinates.Column, ship.Coordinates.Row + i] = ToBoardState(ship.Compartments[i], boardShoot);
            }
        }

        private static void PlaceHorizontal(Ship ship, BoardFieldState[,] board, HashSet<Coordinates> boardShoot)
        {
            for (int i = 0; i < ship.Length.Value; i++)
            {
                board[ship.Coordinates.Column + i, ship.Coordinates.Row] = ToBoardState(ship.Compartments[i], boardShoot);
            }
        }

        static readonly Dictionary<ShipCompartmentState, BoardFieldState> shipCompartmentStateToBoardFieldStateMap = new Dictionary<ShipCompartmentState, BoardFieldState>
        {
            [ShipCompartmentState.Dry] = DTOs.BoardFieldState.Miss,
            [ShipCompartmentState.Flooded] = DTOs.BoardFieldState.Hit,
        };

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
