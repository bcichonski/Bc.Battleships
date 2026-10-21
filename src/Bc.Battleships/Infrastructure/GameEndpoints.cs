using Bc.Battleships.Messages.Commands;
using Bc.Battleships.Messages.CommandsResults;
using Bc.Battleships.Messages.DTOs;
using Bc.Battleships.Messages.Queries;
using Mediator;

namespace Bc.Battleships.Infrastructure
{
    public static class GameEndpoints
    {
        public static IEndpointRouteBuilder MapGameEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/game");

            group.MapGet("", async (IMediator mediator, CancellationToken cancellationToken)
                => await mediator.Send(new GetCurrentGameQuery(), cancellationToken))
                .Produces<GameDto>();

            group.MapPut("", async (ShootCommand shootCommand, IMediator mediator, CancellationToken cancellationToken)
                => await mediator.Send(shootCommand, cancellationToken))
                .Produces<ShootResult>();

            group.MapPost("", async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new CreateNewGameCommand(), cancellationToken);
                return Results.Ok();
            });

            return endpoints;
        }
    }
}
