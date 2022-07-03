using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;

namespace Bc.Battleships.Tests.Common.Builders
{
    public class ShipBuilder
    {
        private Coordinates _coordinates = new Coordinates(1, 1);
        private ShipLayout _layout = ShipLayout.Horizontal;
        private Length _length = new Length(3);
        private ComputerPlayer? _computerPlayer = null;

        public ShipBuilder WithComputerPlayer(ComputerPlayer computerPlayer)
        {
            _computerPlayer = computerPlayer;
            return this;
        }

        public ShipBuilder WithCoordinates(Coordinates coordinates)
        {
            _coordinates = coordinates;
            return this;
        }

        public ShipBuilder WithCoordinates(int x, int y)
        {
            _coordinates = new Coordinates(x, y);
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

        public ShipBuilder WithLength(int length)
        {
            _length = new Length(length);
            return this;
        }


        public Ship Build()
        {
            var ship = new Ship(_coordinates, _layout, _length);

            if(_computerPlayer != null)
            {
                ship.SetOwner(_computerPlayer);
            }

            return ship;
        }
    }
}
