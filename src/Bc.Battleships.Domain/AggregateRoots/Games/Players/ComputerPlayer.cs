using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.Services.FleetFactory;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Domain.ValueObjects.GameContext;

namespace Bc.Battleships.Domain.AggregateRoots.Games.Players;

public class ComputerPlayer : Player
{
    public HitPoints CurrentHitPoints =>
        Ships switch
        {
            not null => new HitPoints(Ships.Sum(x => x.CurrentHitPoints.Value)),
            _ => throw new NullReferenceException()
        };

    public IList<Ship>? Ships { get; private set; } = null;
    public IList<ShotCoordinates> CoordinatesShoot { get; }

    public ComputerPlayer(IList<Ship>? ships)
    {
        Ships = ships;
        CoordinatesShoot = [];
    }

    internal ComputerPlayer()
    {
        CoordinatesShoot = [];
    }

    public void PositionShips(GameContext gameContext, Random? random = null)
    {
        if (Ships is not null)
        {
            throw new ArgumentException("Ships are already positioned.", nameof(Ships));
        }

        ArgumentNullException.ThrowIfNull(gameContext);

        var fleetFactory = new FleetFactory(gameContext, random);

        Ships = fleetFactory.BuildAndPositionShips().ToList();
    }

    public ShootResult IncomingShot(Coordinates coordinates)
    {
        if (CoordinatesShoot.Any(x => x.Coordinates == coordinates))
        {
            return ShootResult.AlreadyShot;
        }

        CoordinatesShoot.Add(new ShotCoordinates(coordinates));

        var targetShip = Ships!.FirstOrDefault(x => x.Occupies(coordinates));

        return targetShip?.Hit(coordinates) ?? ShootResult.Miss;
    }

    public bool IsFloodedAt(Coordinates coordinates)
        => Ships?.Any(x => x.IsFloodedAt(coordinates)) ?? false;
}
