using System;
using AwesomeAssertions;
using System.Linq;
using Bc.Battleships.Application.DTOs;
using Xunit;

namespace Bc.Battleships.UnitTests.Domain.Messages;

public class WhenCreatingBoardDto
{
    private static BoardRowDto[] Rows(int count)
        => Enumerable.Range(0, count).Select(_ => new BoardRowDto([])).ToArray();

    [Fact]
    public void ShouldAcceptMatchingRowCount()
    {
        var dto = new BoardDto(3, Rows(3));

        dto.BoardSize.Should().Be(3);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    public void ShouldThrowArgumentExceptionWhenRowCountDiffersFromBoardSize(int rows)
    {
        var act = () => new BoardDto(3, Rows(rows));

        act.Should().Throw<ArgumentException>();
    }
}
