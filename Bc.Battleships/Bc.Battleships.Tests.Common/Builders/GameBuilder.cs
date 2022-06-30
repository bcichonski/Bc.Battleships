using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.AggregateRoots.Games.Players;
using Bc.Battleships.Tests.Common.Extensions;

namespace Bc.Battleships.Tests.Common.Builders
{
    public class GameBuilder
    {
        private ComputerPlayer? _computerPlayer = null;
        
        public GameBuilder WithComputerPlayer(ComputerPlayer computerPlayer)
        {
            _computerPlayer = computerPlayer;
            return this;
        }

        public Game Build()
        {
            var game = new Game();

            if( _computerPlayer != null )
            {
                game.SetPrivatePropertyValue(nameof(game.ComputerPlayer), _computerPlayer);
            }

            return game;
        }
    }
}
