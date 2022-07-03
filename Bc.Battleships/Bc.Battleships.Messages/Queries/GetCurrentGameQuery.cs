using Bc.Battleships.Messages.DTOs;
using MediatR;

namespace Bc.Battleships.Messages.Queries
{
    public class GetCurrentGameQuery : IRequest<GameDto>
    {
    }
}
