namespace Bc.Battleships.Application.DTOs;

[Serializable]
public class BoardDto
{
    public int BoardSize { get; }
    public BoardRowDto[] BoardRows { get; }

    public BoardDto(int boardSize, BoardRowDto[] boardRows)
    {
        if (boardRows.Length != boardSize)
        {
            throw new ArgumentException($"Board must have exactly {boardSize} rows.", nameof(boardRows));
        }

        BoardSize = boardSize;
        BoardRows = boardRows;
    }

    public BoardDto(BoardRowDto[] boardRows) : this(boardRows.Length, boardRows)
    {
    }
}
