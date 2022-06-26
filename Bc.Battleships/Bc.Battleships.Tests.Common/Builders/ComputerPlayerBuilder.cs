using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.AggregateRoots.Games.Ships;

namespace Bc.Battleships.Tests.Common.Builders
{
    public class ComputerPlayerBuilder
    {
        private IList<Ship>? _ships = CreateDefaultFleet();

        public ComputerPlayerBuilder WithFleet(IList<Ship> ships)
        {
            _ships = ships;
            return this;
        }

        public ComputerPlayerBuilder WithNoFleet()
        {
            _ships = null;
            return this;
        }

        public ComputerPlayer Build()
        {
            var computerPlayer = new ComputerPlayer(_ships);

            return computerPlayer;
        }

        private static IList<Ship>? CreateDefaultFleet()
            => new List<Ship>()
            {
                new ShipBuilder()
                    .Build()
            };
    }
}
