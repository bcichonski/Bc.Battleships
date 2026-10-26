namespace Bc.Battleships.Domain.ValueObjects;

public readonly record struct GameId(Guid Value) : IGuidId
{
    public static GameId New() => new(Guid.NewGuid());
}
