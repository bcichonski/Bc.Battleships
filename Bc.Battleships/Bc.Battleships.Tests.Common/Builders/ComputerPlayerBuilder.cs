using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.AggregateRoots.Games.Ships;

namespace Bc.Battleships.Tests.Common.Builders
{
    public class ComputerPlayerBuilder
    {
        private IList<Ship>? _ships = null;

        public ComputerPlayerBuilder WithFleet(IList<Ship> ships)
        {
            _ships = ships;
            return this;
        }

        public ComputerPlayer Build()
        {
            _ships ??= CreateDefaultFleet();

            var computerPlayer = new ComputerPlayer(_ships!);

            return computerPlayer;
        }

        private IList<Ship>? CreateDefaultFleet()
            => new List<Ship>()
            {
                new ShipBuilder()
                    .Build()
            };
    }
}
