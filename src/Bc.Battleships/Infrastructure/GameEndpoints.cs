using Bc.Battleships.Application.Commands;
using Bc.Battleships.Application.CommandsResults;
using Bc.Battleships.Application.DTOs;
using Bc.Battleships.Application.Queries;
using Bc.Battleships.Domain.ValueObjects;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Bc.Battleships.Infrastructure;

public static class GameEndpoints
{
    private const string GamesRoute = "/games";

    public static IEndpointRouteBuilder MapGameEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(GamesRoute).WithTags("Games");

        group.MapPost("", CreateGame)
            .WithName("CreateGame");

        group.MapGet("{id:guid}", GetGame)
            .WithName("GetGame")
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPost("{id:guid}/shots", Shoot)
            .WithName("Shoot")
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

        // Any other /games/* request (e.g. a malformed id rejected by the route constraint) is a 404 of the API,
        // instead of falling through to the SPA's index.html fallback.
        endpoints.MapFallback($"{GamesRoute}/{{**path}}", () => Results.NotFound())
            .ExcludeFromDescription();

        return endpoints;
    }

    private static async Task<Created<GameDto>> CreateGame(IMediator mediator, CancellationToken cancellationToken)
    {
        var game = await mediator.Send(new CreateNewGameCommand(), cancellationToken);

        return TypedResults.Created($"{GamesRoute}/{game.Id}", game);
    }

    private static async Task<Results<Ok<GameDto>, ProblemHttpResult>> GetGame(
        Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetGameQuery(new GameId(id)), cancellationToken);

        return result.Game is { } game
            ? TypedResults.Ok(game)
            : GameNotFound(id);
    }

    private static async Task<Results<Ok<ShootResult>, ProblemHttpResult>> Shoot(
        Guid id, ShotDto shot, IMediator mediator, CancellationToken cancellationToken)
    {
        var outcome = await mediator.Send(new ShootCommand(new GameId(id), shot.Column, shot.Row), cancellationToken);

        return outcome switch
        {
            ShootOutcome.Shot shotTaken => TypedResults.Ok(shotTaken.Result),
            ShootOutcome.GameNotActive => TypedResults.Problem(
                statusCode: StatusCodes.Status409Conflict,
                title: "Game is not active",
                detail: $"Game '{id}' has finished and accepts no more shots."),
            _ => GameNotFound(id)
        };
    }

    private static ProblemHttpResult GameNotFound(Guid id)
        => TypedResults.Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Game not found",
            detail: $"Game '{id}' does not exist.");
}
