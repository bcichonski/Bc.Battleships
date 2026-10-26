using Bc.Battleships.Application.DTOs;
using Mediator;

namespace Bc.Battleships.Application.Commands;

public sealed record CreateNewGameCommand : IRequest<GameDto>;
