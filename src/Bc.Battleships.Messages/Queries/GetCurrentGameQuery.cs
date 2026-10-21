using Bc.Battleships.Messages.DTOs;
using Mediator;

namespace Bc.Battleships.Messages.Queries
{
    public class GetCurrentGameQuery : IRequest<GameDto>
    {
    }
}
