using Bc.Battleships.Domain.ValueObjects;

namespace Bc.Battleships.Domain.AggregateRoots.Games.Players;

public class ShotCoordinates
{
    public Guid Id { get; }
    public Coordinates Coordinates { get; }

    public ShotCoordinates(Coordinates coordinates)
    {
        Coordinates = coordinates;
    }

    private ShotCoordinates()
    {
        Coordinates = null!;
    }
}
