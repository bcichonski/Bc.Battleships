using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Tests.Common.Extensions;

namespace Bc.Battleships.Tests.Common.Builders
{
    public class GameBuilder
    {
        private ComputerPlayer? _computerPlayer = null;
        private bool _shipsPositioned = true;
        
        public GameBuilder WithComputerPlayer(ComputerPlayer computerPlayer)
        {
            _computerPlayer = computerPlayer;
            return this;
        }

        public GameBuilder WithNotPositionedFleet()
        {
            _shipsPositioned = false;
            return this;
        }

        public Game Build()
        {
            _computerPlayer ??= new ComputerPlayerBuilder()
                .WithFleetPositioned(_shipsPositioned)
                .Build();

            var game = new Game(new GameId(), _computerPlayer);

            _computerPlayer.SetOwner(game);

            return game;
        }
    }
}
