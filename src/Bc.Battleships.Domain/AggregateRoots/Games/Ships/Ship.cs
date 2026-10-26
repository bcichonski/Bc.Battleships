using Bc.Battleships.Domain.Services;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using System.Linq;

namespace Bc.Battleships.Domain.AggregateRoots.Games.Ships;

public class Ship
{
    public ShipId Id { get; }

    public ShipLayout Layout { get; }
    public Coordinates Coordinates { get; }
    public Length Length { get; }
    public IList<ShipCompartment> Compartments { get; }

    public HitPoints CurrentHitPoints => new(Compartments.Where(x => x.State == ShipCompartmentState.Dry).Count());

    public Ship(Coordinates coordinates, ShipLayout layout, Length length)
    {
        Id = ShipId.New();
        Layout = layout;
        Coordinates = coordinates;
        Length = length;

        Compartments = ShipCompartmentsProvider.GetCompartments(this).ToList();
    }

    private Ship()
    {
        Coordinates = null!;
        Length = null!;
        Compartments = null!;
    }

    public ShootResult Hit(Coordinates coordinates)
    {
        var compartment = GetCompartmentAt(coordinates);

        if (compartment is null)
        {
            return ShootResult.Miss;
        }

        if (!compartment.Hit())
        {
            return ShootResult.AlreadyShot;
        }

        return CurrentHitPoints.Value == 0 ? ShootResult.Sunk : ShootResult.Hit;
    }

    public bool Occupies(Coordinates coordinates)
        => GetCompartmentAt(coordinates) is not null;

    public bool IsFloodedAt(Coordinates coordinates)
        => GetCompartmentAt(coordinates)?.State == ShipCompartmentState.Flooded;

    public bool IntersectsWith(Ship ship)
        => Compartments
        .Select(x => x.Coordinates)
        .Concat(ship.Compartments.Select(x => x.Coordinates))
        .Distinct()
        .Count() < Length.Value + ship.Length.Value;

    private ShipCompartment? GetCompartmentAt(Coordinates coordinates)
        => Compartments.FirstOrDefault(x => x.Coordinates == coordinates);
}
