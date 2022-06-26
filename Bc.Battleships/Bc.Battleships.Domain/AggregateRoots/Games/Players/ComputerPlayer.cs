using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.Services.FleetFactory;
using Bc.Battleships.Domain.ValueObjects.GameContext;

namespace Bc.Battleships.Domain.AggregateRoots.Games.Players
{
    public class ComputerPlayer : Player
    {
        protected IList<Ship>? Ships { get; private set; }

        public void PositionShips(GameContext gameContext)
        {
            var fleetFactory = new FleetFactory(gameContext);

            Ships = fleetFactory.BuildAndPositionShips().ToList();
        }
    }
}
