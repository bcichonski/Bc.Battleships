using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.AggregateRoots.Games.Ships;
using Bc.Battleships.Tests.Common.Extensions;

namespace Bc.Battleships.Tests.Common.Builders
{
    public class ComputerPlayerBuilder
    {
        private IList<Ship>? _ships = CreateDefaultFleet();
        private Game? _game;

        public ComputerPlayerBuilder WithGame(Game game)
        {
            _game = game;
            return this;
        }

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
            _game ??= new GameBuilder()
                .Build();

            var computerPlayer = new ComputerPlayer(_game, _ships);

            _game.SetPrivatePropertyValue<ComputerPlayer>(nameof(_game.ComputerPlayer), computerPlayer);

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
