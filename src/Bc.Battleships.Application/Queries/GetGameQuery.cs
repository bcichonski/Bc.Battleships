using Bc.Battleships.Domain.ValueObjects;
using Mediator;

namespace Bc.Battleships.Application.Queries;

public sealed record GetGameQuery(GameId GameId) : IRequest<GetGameQueryResult>;

/// <summary>
/// <see cref="Game"/> is <see langword="null"/> when no game has the requested id.
/// </summary>
public sealed record GetGameQueryResult(DTOs.GameDto? Game);
