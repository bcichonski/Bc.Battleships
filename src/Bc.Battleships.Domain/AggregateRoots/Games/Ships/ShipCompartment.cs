using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;

namespace Bc.Battleships.Domain.AggregateRoots.Games.Ships;

public class ShipCompartment
{
    public ShipCompartmentId Id { get; }
    public Coordinates Coordinates { get; }
    public ShipCompartmentState State { get; private set; }

    public ShipCompartment(Coordinates coordinates)
    {
        Id = ShipCompartmentId.New();
        Coordinates = coordinates;
    }

    private ShipCompartment()
    {
        Coordinates = null!;
    }

    public bool Hit()
    {
        var oldState = State;
        State = ShipCompartmentState.Flooded;

        return oldState == ShipCompartmentState.Dry;
    }
}
