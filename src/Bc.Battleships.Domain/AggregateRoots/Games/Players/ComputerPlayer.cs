using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.Services.FleetFactory;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Domain.ValueObjects.GameContext;

namespace Bc.Battleships.Domain.AggregateRoots.Games.Players
{
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
            CoordinatesShoot = new List<ShotCoordinates>();
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal ComputerPlayer() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public void PositionShips(GameContext gameContext)
        {
            if (Ships is not null)
            {
                throw new ArgumentException("Ships are already positioned.", nameof(Ships));
            }

            ArgumentNullException.ThrowIfNull(gameContext);

            var fleetFactory = new FleetFactory(gameContext);

            Ships = fleetFactory.BuildAndPositionShips().ToList();
        }

        public ShootResult IncomingShot(Coordinates coordinates)
        {
            if(CoordinatesShoot.All(x => x.Coordinates != coordinates))
            {
                CoordinatesShoot.Add(new ShotCoordinates(coordinates));
            }

            foreach(var ship in Ships!)
            {
                if(ship.Hit(coordinates) == ShootResult.Hit)
                {
                    return ShootResult.Hit;
                }
            }

            return ShootResult.Miss;
        }
    }
}
