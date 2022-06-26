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

        public ComputerPlayer() : base()
        {
        }

        internal ComputerPlayer(IList<Ship>? ships)
        {
            Ships = ships;
        }

        public void PositionShips(GameContext gameContext)
        {
            Ensure.That(Ships is null).IsTrue();

            var fleetFactory = new FleetFactory(gameContext);

            Ships = fleetFactory.BuildAndPositionShips().ToList();
        }

        public ShootResult BraceForImpact(Coordinates coordinates)
        {
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
