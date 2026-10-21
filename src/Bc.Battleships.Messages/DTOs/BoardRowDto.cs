namespace Bc.Battleships.Messages.DTOs
{
    [Serializable]
    public class BoardRowDto
    {
        public BoardFieldState[] Row { get; }

        public BoardRowDto(BoardFieldState[] row)
        {
            Row = row;
        }
    }
}
