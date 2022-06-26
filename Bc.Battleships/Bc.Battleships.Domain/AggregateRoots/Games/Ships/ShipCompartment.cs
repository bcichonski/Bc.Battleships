using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;

namespace Bc.Battleships.Domain.AggregateRoots.Games.Ships
{
    public class ShipCompartment
    {
        public ShipCompartmentId Id { get; }
        public Coordinates Coordinates { get; }
        public ShipCompartmentState State { get; }

        public ShipCompartment(Coordinates coordinates)
        {
            Id = new ShipCompartmentId();
            Coordinates = coordinates;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ShipCompartment() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
