using EnsureThat;

namespace Bc.Battleships.Messages.DTOs
{
    public class BoardDto
    {
        public int BoardSize { get; }
        public BoardFieldState[,] BoardFieldState { get; }

        public BoardDto(int boardSize, BoardFieldState[,] boardFieldState)
        {
            Ensure.That(boardFieldState.Rank).Is(2);
            Ensure.That(boardFieldState.GetLength(0)).Equals(boardSize);
            Ensure.That(boardFieldState.GetLength(1)).Equals(boardSize);

            BoardSize = boardSize;
            BoardFieldState = boardFieldState;
        }

        public BoardDto(BoardFieldState[,] boardFieldState) : this(boardFieldState.GetLength(0), boardFieldState)
        {
        }
    }
}
