using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.Enums;
using Bc.Battleships.Domain.ValueObjects.GameContext;

namespace Bc.Battleships.Domain.Services.FleetFactory
{
    internal class FleetFactory
    {
        private readonly Random _random;
        private readonly GameContext _gameContext;

        public FleetFactory(GameContext gameContext, Random? random = null)
        {
            _gameContext = gameContext;
            _random = random ?? Random.Shared;
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
            var candidates = new List<Ship>();
            var boardSize = _gameContext.BoardSize.Value;
            var length = shipBlueprint.Length.Value;

            foreach (var layout in Enum.GetValues<ShipLayout>())
            {
                var maxColumn = layout == ShipLayout.Horizontal ? boardSize - length + 1 : boardSize;
                var maxRow = layout == ShipLayout.Vertical ? boardSize - length + 1 : boardSize;

                for (var column = 1; column <= maxColumn; column++)
                {
                    for (var row = 1; row <= maxRow; row++)
                    {
                        var ship = new Ship(new Coordinates(column, row), layout, shipBlueprint.Length);

                        if (IsNotCollidingWith(ship, createdShips))
                        {
                            candidates.Add(ship);
                        }
                    }
                }
            }

            if (candidates.Count == 0)
            {
                throw new FleetFactoryPositioningFailedException();
            }

            return candidates[_random.Next(candidates.Count)];
        }

        private static bool IsNotCollidingWith(Ship ship, List<Ship> createdShips)
            => createdShips.All(x => !x.IntersectsWith(ship));
    }
}
