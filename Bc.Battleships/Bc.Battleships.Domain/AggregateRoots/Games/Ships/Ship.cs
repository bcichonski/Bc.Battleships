using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.Events;
using Bc.Battleships.Domain.Events.Ships;
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

        public ShootResult Hit(Coordinates coordinates)
        {
            if(GetCompartmentAt(coordinates)?.Hit() != null)
            {
                EmitEventOnSinking();

                return ShootResult.Hit;
            }

            return ShootResult.Miss;
        }

        private void EmitEventOnSinking()
        {
            if (CurrentHitPoints.Value <= 0)
            {
                DomainEventBus.Instance.Publish(new AbandonTheShipDomainEvent(this));
            }
        }

        public bool IntersectsWith(Ship ship)
            => Compartments
            .Select(x => x.Coordinates)
            .Concat(ship.Compartments.Select(x => x.Coordinates))
            .Distinct()
            .Count() < Length.Value + ship.Length.Value;

        public bool IntersectsWith(Coordinates coordinates)
            => GetCompartmentAt(coordinates) != null;

        private ShipCompartment? GetCompartmentAt(Coordinates coordinates)
            => Compartments.FirstOrDefault(x => x.Coordinates == coordinates);
    }
}
