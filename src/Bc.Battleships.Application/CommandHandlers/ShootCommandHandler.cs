using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.Repositories;
using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Application.Commands;
using Bc.Battleships.Application.CommandsResults;
using Mediator;
using GameState = Bc.Battleships.Domain.ValueObjects.Enums.GameState;
using ShootResultEnum = Bc.Battleships.Domain.ValueObjects.Enums.ShootResult;

namespace Bc.Battleships.Application.CommandHandlers;

internal class ShootCommandHandler(IGamesRepository gamesRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<ShootCommand, ShootOutcome>
{

    public async ValueTask<ShootOutcome> Handle(ShootCommand request, CancellationToken cancellationToken)
    {
        var game = await gamesRepository.GetById(request.GameId, cancellationToken);

        if (game is null)
        {
            return new ShootOutcome.GameNotFound();
        }

        // Rejected before the aggregate is touched: nothing is recorded and nothing is saved.
        if (game.GameState != GameState.Active)
        {
            return new ShootOutcome.GameNotActive();
        }

        var coordinates = new Coordinates(request.Column + 1, request.Row + 1);
        var result = game.Shoot(coordinates);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ShootOutcome.Shot(
            new ShootResult(IsCellHit(game, coordinates, result), game.ComputerPlayer.CurrentHitPoints.Value));
    }

    private static bool IsCellHit(Game game, Coordinates coordinates, ShootResultEnum result)
        => result switch
        {
            ShootResultEnum.Hit or ShootResultEnum.Sunk => true,
            ShootResultEnum.AlreadyShot => game.ComputerPlayer.IsFloodedAt(coordinates),
            _ => false
        };
}
