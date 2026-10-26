namespace Bc.Battleships.Domain.ValueObjects;

public readonly record struct ShipCompartmentId(Guid Value) : IGuidId
{
    public static ShipCompartmentId New() => new(Guid.NewGuid());
}
