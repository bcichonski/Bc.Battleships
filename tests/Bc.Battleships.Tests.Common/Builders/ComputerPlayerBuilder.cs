using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.ValueObjects;

namespace Bc.Battleships.Tests.Common.Builders
{
    public class ComputerPlayerBuilder
    {
        private ISet<Coordinates>? _coordinatesShot = new HashSet<Coordinates>() { new Coordinates(1, 1) };
        private IList<Ship>? _ships = CreateDefaultFleet();

        public ComputerPlayerBuilder WithFleet(IList<Ship> ships)
        {
            _ships = ships;
            return this;
        }

        public ComputerPlayerBuilder WithFleetPositioned(bool positioned)
        {
            if (!positioned)
            {
                _ships = null;
            }

            return this;
        }

        public ComputerPlayerBuilder WithSetOfCoordinatesShot(ISet<Coordinates> coordinatesShot)
        {
            _coordinatesShot = coordinatesShot;
            return this;
        }

        public ComputerPlayer Build()
        {
            var computerPlayer = new ComputerPlayer(_ships);

            if (_ships != null)
            {
                if (_coordinatesShot != null)
                {
                    ShootAll(computerPlayer, _coordinatesShot);
                }
            }

            return computerPlayer;
        }

        private static void ShootAll(ComputerPlayer computerPlayer, ISet<Coordinates> coordinatesShot)
        {
            foreach (var coordinates in coordinatesShot)
            {
                computerPlayer.IncomingShot(coordinates);
            }
        }

        private static IList<Ship>? CreateDefaultFleet()
            => new List<Ship>()
            {
                new ShipBuilder()
                    .Build()
            };
    }
}
