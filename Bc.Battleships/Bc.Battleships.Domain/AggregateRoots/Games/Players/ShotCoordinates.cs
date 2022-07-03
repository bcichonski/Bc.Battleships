using Bc.Battleships.Domain.ValueObjects;

namespace Bc.Battleships.Domain.AggregateRoots.Games.Players
{
    public class ShotCoordinates
    {
        public Guid Id { get; }
        public Coordinates Coordinates { get; }

        public ShotCoordinates(Coordinates coordinates)
        {
            Coordinates = coordinates;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ShotCoordinates() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
