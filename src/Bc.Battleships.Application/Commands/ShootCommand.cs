using Bc.Battleships.Application.CommandsResults;
using Bc.Battleships.Domain.ValueObjects;
using Mediator;

namespace Bc.Battleships.Application.Commands;

/// <summary>
/// A shot at the computer player's board of the game <paramref name="GameId"/>.
/// <paramref name="Column"/> and <paramref name="Row"/> are zero-based.
/// </summary>
public sealed record ShootCommand(GameId GameId, int Column, int Row) : IRequest<ShootOutcome>;
