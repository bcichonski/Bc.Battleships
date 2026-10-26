using AwesomeAssertions;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Exceptions;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.ValueObjects;

public class WhenCreatingLength
{
    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    public void ShouldAcceptNonNegativeValue(int value)
    {
        new Length(value).Value.Should().Be(value);
    }

    [Fact]
    public void ShouldThrowForNegativeValue()
    {
        var act = () => new Length(-1);

        act.Should().Throw<NegativeLengthValueException>();
    }
}
