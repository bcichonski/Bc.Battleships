using EnsureThat;

namespace Bc.Battleships.Server.DTOs
{
    public class BoardDto
    {
        public int BoardSize { get; }
        public BoardFieldState[,] BoardFieldState { get; }

        public BoardDto(int boardSize, BoardFieldState[,] boardFieldState)
        {
            Ensure.That(boardFieldState.Length).Equals(boardSize);

            BoardSize = boardSize;
            BoardFieldState = boardFieldState;
        }

        public BoardDto(BoardFieldState[,] boardFieldState) : this(boardFieldState.Length, boardFieldState)
        {
        }
    }
}
