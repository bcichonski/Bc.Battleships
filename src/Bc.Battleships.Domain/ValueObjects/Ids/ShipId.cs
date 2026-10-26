namespace Bc.Battleships.Domain.ValueObjects;

public readonly record struct ShipId(Guid Value) : IGuidId
{
    public static ShipId New() => new(Guid.NewGuid());
}
