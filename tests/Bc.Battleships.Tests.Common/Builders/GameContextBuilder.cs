using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.GameContext;

namespace Bc.Battleships.Tests.Common.Builders;

public static class GameContextBuilder
{
    public static GameContext Standard { get; } = Create(10, 4, 4, 5);

    public static GameContext Create(int boardSize, params int[] shipLengths)
        => new(new Length(boardSize), shipLengths.Select(x => new ShipBlueprint(new Length(x))).ToList());
}
