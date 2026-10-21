using Bc.Battleships.Messages.CommandsResults;
using Mediator;

namespace Bc.Battleships.Messages.Commands
{
    public class ShootCommand : IRequest<ShootResult>
    {
        public int Column { get; }
        public int Row { get; }

        public ShootCommand(int column, int row)
        {
            Column = column;
            Row = row;
        }
    }
}
