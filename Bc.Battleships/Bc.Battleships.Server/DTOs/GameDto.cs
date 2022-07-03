using Bc.Battleships.Domain.AggregateRoots.Games;

namespace Bc.Battleships.Server.DTOs
{
    public class GameDto
    {
        public BoardDto Board { get; }

        public int CurrentHitPoints { get; }

        public GameDto(BoardDto board, int currentHitPoints)
        {
            Board = board;
            CurrentHitPoints = currentHitPoints;
        }
    }
}
