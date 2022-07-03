using EnsureThat;
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
        public IList<ShotCoordinates> CoordinatesShoot { get; } = new List<ShotCoordinates>();

        public Game? Game { get; private set; }

        public ComputerPlayer(Game? game, IList<Ship>? ships)
        {
            Game = game;
            Ships = ships;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ComputerPlayer() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        internal void SetOwner(Game game)
        {
            Game = game;
        }

        public void PositionShips()
        {
            Ensure.That(Ships is null).IsTrue();
            Ensure.That(Game).IsNotNull();

#pragma warning disable CS8604 // Possible null reference argument.
            var fleetFactory = new FleetFactory(Game);
#pragma warning restore CS8604 // Possible null reference argument.

            Ships = fleetFactory.BuildAndPositionShips().ToList();
        }

        public ShootResult IncomingShot(Coordinates coordinates)
        {
            if(CoordinatesShoot.All(x => x.Coordinates != coordinates))
            {
                CoordinatesShoot.Add(new ShotCoordinates(this, coordinates));
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
