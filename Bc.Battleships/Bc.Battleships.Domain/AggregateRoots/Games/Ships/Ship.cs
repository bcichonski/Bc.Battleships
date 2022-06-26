using Bc.Battleships.Domain.Services;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using System.Linq;

namespace Bc.Battleships.Domain.AggregateRoots.Games.Ships
{
    public class Ship
    {
        public ShipLayout Layout { get; }
        public Coordinates Coordinates { get; }
        public Length Length { get; }
        public IList<ShipCompartment> Compartments { get; }
        public HitPoints CurrentHitPoints => new HitPoints(Compartments.Where(x => x.State == ShipCompartmentState.Dry).Count());

        public Ship(Coordinates coordinates, ShipLayout layout, Length length)
        {
            Layout = layout;
            Coordinates = coordinates;
            Length = length;

            Compartments = ShipCompartmentsProvider.GetCompartments(this).ToList();
        }

        public bool CollidesWith(Ship ship)
            => Compartments
            .Select(x => x.Coordinates)
            .Concat(ship.Compartments.Select(x => x.Coordinates))
            .Distinct()
            .Count() < Length.Value + ship.Length.Value;
    }
}
