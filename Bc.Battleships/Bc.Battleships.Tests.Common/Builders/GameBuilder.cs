using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.AggregateRoots.Games.Players;

namespace Bc.Battleships.Tests.Common.Builders
{
    public class GameBuilder
    {
        private ComputerPlayer? _computerPlayer = null;

        public Game Build()
        {
            _computerPlayer ??= new ComputerPlayerBuilder()
                .Build();

            var game = new Game(_computerPlayer);

            return game;
        }
    }
}
