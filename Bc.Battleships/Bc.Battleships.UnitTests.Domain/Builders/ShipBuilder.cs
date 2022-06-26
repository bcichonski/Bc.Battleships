using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;

namespace Bc.Battleships.UnitTests.Domain.Builders
{
    internal class ShipBuilder
    {
        private Coordinates _coordinates = new Coordinates(1, 1);
        private ShipLayout _layout = ShipLayout.Horizontal;
        private Length _length = new Length(3);

        public ShipBuilder WithCoordinates(Coordinates coordinates)
        {
            _coordinates = coordinates;
            return this;
        }

        public ShipBuilder WithLayout(ShipLayout layout)
        {
            _layout = layout;
            return this;
        }

        public ShipBuilder WithLength(Length length)
        {
            _length = length;
            return this;
        }


        public Ship Build()
        {
            return new Ship(_coordinates, _layout, _length);
        }
    }
}
