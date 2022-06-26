using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;

namespace Bc.Battleships.Domain.AggregateRoots.Games.Ships
{
    public class ShipCompartment
    {
        public Coordinates Coordinates { get; }
        public ShipCompartmentState State { get; }

        public ShipCompartment(Coordinates coordinates)
        {
            Coordinates = coordinates;
        }
    }
}
