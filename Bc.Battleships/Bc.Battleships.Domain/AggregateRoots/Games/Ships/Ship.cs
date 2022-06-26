using Bc.Battleships.Domain.Enums;
using Bc.Battleships.Domain.ValueObjects;

namespace Bc.Battleships.Domain.AggregateRoots.Games.Ships
{
    public class Ship
    {
        public ShipLayout Layout { get; }
        public Coordinates Coordinates { get; }
        public Length Length { get; }
    }
}
