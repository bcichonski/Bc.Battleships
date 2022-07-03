using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Server.CommandsResults;
using MediatR;

namespace Bc.Battleships.Server.Commands
{
    public class ShootCommand : IRequest<ShootResult>
    {
        public Coordinates Coordinates { get; }

        public ShootCommand(Coordinates coordinates)
        {
            Coordinates = coordinates;
        }
    }
}
