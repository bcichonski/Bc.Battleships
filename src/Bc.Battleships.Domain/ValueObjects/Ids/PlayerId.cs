namespace Bc.Battleships.Domain.ValueObjects;

public readonly record struct PlayerId(Guid Value) : IGuidId
{
    public static PlayerId New() => new(Guid.NewGuid());
}
