using FluentAssertions;
using Bc.Battleships.Domain.ValueObjects;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Services
{
    public class WhenConvertingCoordinates
    {
        [Theory]
        [InlineData(1, 2, "A2")]
        [InlineData(10, 1, "J1")]
        public void ShouldConvertCooridinatesToString(int column, int row, string expectedString)
        {
            var coordinatesString = new Coordinates(column, row).ToString();

            coordinatesString.Should().Be(expectedString);
        }

        [Theory]
        [InlineData("A2", 1, 2)]
        [InlineData("J1", 10, 1)]
        public void ShouldConvertStringToCoordinates(string @string, int expectedColumn, int expectedRow)
        {
            var coordinates = Coordinates.Parse(@string);

            coordinates.X.Should().Be(expectedColumn);
            coordinates.Y.Should().Be(expectedRow);
        }
    }
}
