using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Application.Commands;
using Bc.Battleships.Application.DTOs;
using Bc.Battleships.Application.Options;
using Bc.Battleships.Application.Services;
using Mediator;
using Microsoft.Extensions.Options;

namespace Bc.Battleships.Application.CommandHandlers;

public class CreateNewGameCommandHandler(
    IGamesRepository gamesRepository,
    IUnitOfWork unitOfWork,
    IOptions<GameOptions> options) : IRequestHandler<CreateNewGameCommand, GameDto>
{

    public async ValueTask<GameDto> Handle(CreateNewGameCommand request, CancellationToken cancellationToken)
    {
        var gameOptions = options.Value;
        var newGame = Game.CreateNewGame(gameOptions.ToGameContext());

        gamesRepository.Add(newGame);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return GameToGameDtoConverter.Convert(newGame, gameOptions.BoardSize);
    }
}
