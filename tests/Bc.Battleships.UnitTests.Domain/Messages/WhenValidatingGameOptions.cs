using AwesomeAssertions;
using Bc.Battleships.Application.Options;
using System.Linq;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Messages;

public class WhenValidatingGameOptions
{
    private static bool IsValid(GameOptions options)
        => new GameOptionsValidator().Validate(null, options).Succeeded;

    [Fact]
    public void DefaultsShouldBeValid()
    {
        var options = new GameOptions();

        IsValid(options).Should().BeTrue();
        options.BoardSize.Should().Be(10);
        options.EffectiveShipLengths.Should().Equal(4, 4, 5);
    }

    [Fact]
    public void DefaultsShouldBuildTodaysGameContext()
    {
        var context = new GameOptions().ToGameContext();

        context.BoardSize.Value.Should().Be(10);
        context.ShipBlueprints.Select(x => x.Length.Value).Should().Equal(4, 4, 5);
    }

    [Fact]
    public void BoardSizeZeroShouldBeInvalid()
        => IsValid(new GameOptions { BoardSize = 0 }).Should().BeFalse();

    [Fact]
    public void NoShipsShouldBeInvalid()
        => IsValid(new GameOptions { ShipLengths = [] }).Should().BeFalse();

    [Fact]
    public void ShipLongerThanBoardShouldBeInvalid()
        => IsValid(new GameOptions { BoardSize = 4, ShipLengths = [5] }).Should().BeFalse();

    [Fact]
    public void ShipOfLengthZeroShouldBeInvalid()
        => IsValid(new GameOptions { ShipLengths = [0] }).Should().BeFalse();
}
