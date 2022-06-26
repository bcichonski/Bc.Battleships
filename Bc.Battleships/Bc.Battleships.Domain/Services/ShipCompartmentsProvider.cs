using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;

namespace Bc.Battleships.Domain.Services
{
    static class ShipCompartmentsProvider
    {
        public static IEnumerable<ShipCompartment> GetCompartments(Ship ship)
        {
            var constantDimensionValue = ship.Layout == ShipLayout.Horizontal ? ship.Coordinates.X : ship.Coordinates.Y;
            var varyingDimensionValue = ship.Layout == ShipLayout.Horizontal ? ship.Coordinates.Y : ship.Coordinates.X;

            var varyingDimensionValues = Enumerable.Range(varyingDimensionValue, ship.Length.Value);

            return varyingDimensionValues.Select(x => CreateCompartment(constantDimensionValue, x, ship.Layout));
        }

        private static ShipCompartment CreateCompartment(int constantDimensionValue, int varyingDimensionValue, ShipLayout layout)
            => layout == ShipLayout.Horizontal 
                ? new ShipCompartment(new Coordinates(varyingDimensionValue, constantDimensionValue))
                : new ShipCompartment(new Coordinates(constantDimensionValue, varyingDimensionValue));
    }
}
