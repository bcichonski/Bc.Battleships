using Bc.Battleships.Domain.ValueObjects;
using MediatR;

namespace Bc.Battleships.Server.Commands
{
    public class ShootRequest : IRequest
    {
        public Coordinates Coordinates { get; }

        public ShootRequest(Coordinates coordinates)
        {
            Coordinates = coordinates;
        }
    }
}
