using Bc.Battleships.Server.DTOs;
using MediatR;

namespace Bc.Battleships.Server.Queries
{
    public class GetCurrentGameQuery : IRequest<GameDto>
    {
    }
}
