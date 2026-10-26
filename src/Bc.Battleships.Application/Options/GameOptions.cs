using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.GameContext;

namespace Bc.Battleships.Application.Options;

public class GameOptions
{
    public const string SectionName = "Game";

    public int BoardSize { get; set; } = 10;

    private static readonly int[] DefaultShipLengths = [4, 4, 5];

    /// <summary>
    /// Left null so configuration binding replaces the default fleet instead of appending to it.
    /// </summary>
    public int[]? ShipLengths { get; set; }

    public IReadOnlyList<int> EffectiveShipLengths => ShipLengths ?? DefaultShipLengths;

    public GameContext ToGameContext()
        => new(
            new Length(BoardSize),
            EffectiveShipLengths.Select(length => new ShipBlueprint(new Length(length))).ToList());
}
