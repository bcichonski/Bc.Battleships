using System;
using AwesomeAssertions;
using Bc.Battleships.Domain.ValueObjects;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.ValueObjects;

public class WhenComparingIds
{
    [Fact]
    public void IdsWithTheSameValueShouldBeEqual()
    {
        var value = Guid.NewGuid();

        new GameId(value).Should().Be(new GameId(value));
        new ShipId(value).Should().Be(new ShipId(value));
    }

    [Fact]
    public void NewIdsShouldDiffer()
    {
        GameId.New().Should().NotBe(GameId.New());
        PlayerId.New().Value.Should().NotBeEmpty();
    }
}
