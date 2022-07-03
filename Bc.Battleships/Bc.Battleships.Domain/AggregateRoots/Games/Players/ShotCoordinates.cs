using Bc.Battleships.Domain.ValueObjects;

namespace Bc.Battleships.Domain.AggregateRoots.Games.Players
{
    public class ShotCoordinates
    {
        public ComputerPlayer Owner { get; }

        public Coordinates Coordinates { get; }

        public ShotCoordinates(ComputerPlayer owner, Coordinates coordinates)
        {
            Owner = owner;
            Coordinates = coordinates;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ShotCoordinates() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
