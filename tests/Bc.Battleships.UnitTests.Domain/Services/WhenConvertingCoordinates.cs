using AwesomeAssertions;
using Bc.Battleships.Domain.ValueObjects;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Services
{
    public class WhenConvertingCoordinates
    {
        [Theory]
        [InlineData(2, 1, "A2")]
        [InlineData(1, 10, "J1")]
        [InlineData(10, 1, "A10")]
        [InlineData(4, 4, "D4")]
        public void ShouldConvertCooridinatesToString(int column, int row, string expectedString)
        {
            var coordinatesString = new Coordinates(column, row).ToString();

            coordinatesString.Should().Be(expectedString);
        }

        [Theory]
        [InlineData("A2", 2, 1)]
        [InlineData("J1", 1, 10)]
        [InlineData("A10", 10, 1)]
        [InlineData("D4", 4, 4)]
        public void ShouldConvertStringToCoordinates(string @string, int expectedColumn, int expectedRow)
        {
            var coordinates = Coordinates.Parse(@string);

            coordinates.Column.Should().Be(expectedColumn);
            coordinates.Row.Should().Be(expectedRow);
        }
    }
}
