namespace Bc.Battleships.Application.CommandsResults;

/// <summary>
/// What happened to a <see cref="Commands.ShootCommand"/>: the shot was taken, or it was rejected
/// before the game was touched.
/// </summary>
public abstract record ShootOutcome
{
    private ShootOutcome()
    {
    }

    public sealed record Shot(ShootResult Result) : ShootOutcome;

    public sealed record GameNotFound : ShootOutcome;

    public sealed record GameNotActive : ShootOutcome;
}
