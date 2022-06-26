using Bc.Battleships.Domain.Services;

namespace Bc.Battleships.Domain.ValueObjects
{
    public record Coordinates(int Column, int Row)
    {
        public override string ToString()
        {
            return $"{NumberToLettersConverter.Convert(Column)}{Row}";
        }

        public static Coordinates Parse(string value)
        {
            var columnString = value.Substring(0, 1);
            var rowString = value.Substring(1);

            var column = NumberToLettersConverter.Convert(columnString);
            var row = int.Parse(rowString);

            return new Coordinates(column, row);
        }
    }
}
