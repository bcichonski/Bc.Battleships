using Microsoft.Extensions.Options;

namespace Bc.Battleships.Application.Options;

public class GameOptionsValidator : IValidateOptions<GameOptions>
{
    public ValidateOptionsResult Validate(string? name, GameOptions options)
    {
        List<string> failures = [];

        if (options.BoardSize < 1)
        {
            failures.Add($"{GameOptions.SectionName}:{nameof(GameOptions.BoardSize)} must be at least 1.");
        }

        var shipLengths = options.EffectiveShipLengths;

        if (shipLengths.Count == 0)
        {
            failures.Add($"{GameOptions.SectionName}:{nameof(GameOptions.ShipLengths)} must contain at least one ship.");
        }
        else if (shipLengths.Any(length => length < 1 || length > options.BoardSize))
        {
            failures.Add($"{GameOptions.SectionName}:{nameof(GameOptions.ShipLengths)} lengths must be between 1 and the board size ({options.BoardSize}).");
        }

        return failures.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(failures);
    }
}
