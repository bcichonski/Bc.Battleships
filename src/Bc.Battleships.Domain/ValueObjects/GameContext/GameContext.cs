namespace Bc.Battleships.Domain.ValueObjects.GameContext;

public record GameContext(Length BoardSize, IReadOnlyList<ShipBlueprint> ShipBlueprints);
