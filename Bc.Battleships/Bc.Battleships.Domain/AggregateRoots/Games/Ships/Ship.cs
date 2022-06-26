using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.Services;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using System.Linq;

namespace Bc.Battleships.Domain.AggregateRoots.Games.Ships
{
    public class Ship
    {
        public ShipId Id { get; }
        public ShipLayout Layout { get; }
        public Coordinates Coordinates { get; }
        public Length Length { get; }
        public IList<ShipCompartment> Compartments { get; }
        public HitPoints CurrentHitPoints => new HitPoints(Compartments.Where(x => x.State == ShipCompartmentState.Dry).Count());

        public Ship(Coordinates coordinates, ShipLayout layout, Length length)
        {
            Id = new ShipId();
            Layout = layout;
            Coordinates = coordinates;
            Length = length;

            Compartments = ShipCompartmentsProvider.GetCompartments(this).ToList();
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Ship()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        { }

        public bool CollidesWith(Ship ship)
            => Compartments
            .Select(x => x.Coordinates)
            .Concat(ship.Compartments.Select(x => x.Coordinates))
            .Distinct()
            .Count() < Length.Value + ship.Length.Value;
    }
}
