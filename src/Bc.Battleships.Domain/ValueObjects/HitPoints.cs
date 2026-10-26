namespace Bc.Battleships.Domain.ValueObjects;

public record HitPoints(int Value)
{
    public HitPoints Substract(int argument)
        => new HitPoints(Value - argument);
}
