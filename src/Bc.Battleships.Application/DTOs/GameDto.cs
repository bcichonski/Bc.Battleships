namespace Bc.Battleships.Application.DTOs;

public class GameDto
{
    public Guid Id { get; }

    public bool IsGameActive { get; }

    public int CurrentHitPoints { get; }

    public BoardDto Board { get; }

    public GameDto(Guid id, bool isGameActive, BoardDto board, int currentHitPoints)
    {
        Id = id;
        IsGameActive = isGameActive;
        Board = board;
        CurrentHitPoints = currentHitPoints;
    }
}
