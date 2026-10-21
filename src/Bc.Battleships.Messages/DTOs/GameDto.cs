namespace Bc.Battleships.Messages.DTOs
{
    public class GameDto
    {
        public BoardDto? Board { get; }

        public int CurrentHitPoints { get; }
        public bool IsGameActive { get; set; }

        public GameDto(bool isGameActive, BoardDto? board, int currentHitPoints)
        {
            IsGameActive = isGameActive;
            Board = board;
            CurrentHitPoints = currentHitPoints;
        }
    }
}
