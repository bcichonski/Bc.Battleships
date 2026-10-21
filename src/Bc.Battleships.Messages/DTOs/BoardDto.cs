using EnsureThat;

namespace Bc.Battleships.Messages.DTOs
{
    [Serializable]
    public class BoardDto
    {
        public int BoardSize { get; }
        public BoardRowDto[] BoardRows { get; }

        public BoardDto(int boardSize, BoardRowDto[] boardRows)
        {
            Ensure.That(boardRows.Length).Is(boardSize);

            BoardSize = boardSize;
            BoardRows = boardRows;
        }

        public BoardDto(BoardRowDto[] boardRows) : this(boardRows.Length, boardRows)
        {
        }
    }
}
