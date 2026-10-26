using Bc.Battleships.Domain.Services;

namespace Bc.Battleships.Domain.ValueObjects;

public record Coordinates(int Column, int Row)
{
    public override string ToString()
    {
        return $"{NumberToLettersConverter.Convert(Row)}{Column}";
    }

    public static Coordinates Parse(string value)
    {
        var columnString = value[1..];
        var rowString = value[..1];

        var column = int.Parse(columnString);
        var row = NumberToLettersConverter.Convert(rowString);

        return new Coordinates(column, row);
    }
}
