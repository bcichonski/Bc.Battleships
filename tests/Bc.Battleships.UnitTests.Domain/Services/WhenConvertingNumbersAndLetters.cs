using System;
using AwesomeAssertions;
using Bc.Battleships.Domain.Services;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Services
{
    public class WhenConvertingNumbersAndLetters
    {
        [Theory]
        [InlineData(1, "A")]
        [InlineData(25, "Z")]
        public void ShouldConvertNumberToLetterAtBoundaries(int number, string expected)
        {
            NumberToLettersConverter.Convert(number).Should().Be(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(26)]
        public void ShouldThrowArgumentOutOfRangeForNumberOutsideRange(int number)
        {
            var act = () => NumberToLettersConverter.Convert(number);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData("A", 1)]
        [InlineData("Z", 25)]
        public void ShouldConvertLetterToNumber(string letter, int expected)
        {
            NumberToLettersConverter.Convert(letter).Should().Be(expected);
        }

        [Fact]
        public void ShouldThrowArgumentNullForNullLetter()
        {
            var act = () => NumberToLettersConverter.Convert((string)null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("AB")]
        public void ShouldThrowArgumentExceptionForBlankOrLongLetter(string letter)
        {
            var act = () => NumberToLettersConverter.Convert(letter);

            act.Should().Throw<ArgumentException>().Which.Should().NotBeOfType<ArgumentNullException>();
        }
    }
}
