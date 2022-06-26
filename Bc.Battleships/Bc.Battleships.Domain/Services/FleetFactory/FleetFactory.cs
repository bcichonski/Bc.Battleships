using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Domain.ValueObjects.GameContext;
using Bc.Battleships.Domain.Extensions;

namespace Bc.Battleships.Domain.Services.FleetFactory
{
    internal class FleetFactory
    {
        private readonly Random _random = new Random();
        private readonly GameContext _gameContext;

        public FleetFactory(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public IEnumerable<Ship> BuildAndPositionShips()
        {
            var createdShips = new List<Ship>();

            foreach (var shipBlueprint in _gameContext.ShipBlueprints)
            {
                createdShips.Add(BuildAndPositionShip(shipBlueprint, createdShips));
            }

            return createdShips;
        }

        private Ship BuildAndPositionShip(ShipBlueprint shipBlueprint, List<Ship> createdShips)
        {
            const int maxRandomAttempts = 10;
            int currentAttempt = 1;

            while (currentAttempt < maxRandomAttempts)
            {

                var ship = BuildAndPositionShipRandomly(shipBlueprint);

                if (IsNotCollidingWith(ship, createdShips))
                {
                    return ship;
                }

                currentAttempt++;
            }

            throw new FleetFactoryPositioningFailedException();
        }

        private bool IsNotCollidingWith(Ship ship, List<Ship> createdShips)
            => createdShips.All(x => !x.IntersectsWith(ship));

        private Ship BuildAndPositionShipRandomly(ShipBlueprint shipBlueprint)
        {
            var randomLayout = GetRandomLayout();
            var randomCoordinates = GetRandomCoordinates(randomLayout, shipBlueprint);

            return new Ship(randomCoordinates, randomLayout, shipBlueprint.Length);
        }

        private ShipLayout GetRandomLayout()
            => EnumExtensions.GetRandomValue<ShipLayout>(_random);

        private Coordinates GetRandomCoordinates(ShipLayout shipLayout, ShipBlueprint shipBlueprint)
            => shipLayout switch
            {
                ShipLayout.Horizontal => new Coordinates(_random.Next(1, _gameContext.BoardSize.Value - shipBlueprint.Length.Value), _random.Next(1, _gameContext.BoardSize.Value)),
                ShipLayout.Vertical => new Coordinates(_random.Next(1, _gameContext.BoardSize.Value), _random.Next(1, _gameContext.BoardSize.Value - shipBlueprint.Length.Value)),
                _ => throw new FleetFactoryException()
            };
    }
}
