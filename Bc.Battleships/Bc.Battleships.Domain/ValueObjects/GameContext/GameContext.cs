using Bc.Battleships.Domain.ValueObjects;

namespace Bc.Battleships.Domain.ValueObjects.GameContext
{
    public record GameContext(Length BoardSize)
    {
        public IList<ShipBlueprint> ShipBlueprints { get; } = new List<ShipBlueprint>()
        {
            new ShipBlueprint(new Length(3)),
            new ShipBlueprint(new Length(3)),
            new ShipBlueprint(new Length(4)),
        };
    }
}
