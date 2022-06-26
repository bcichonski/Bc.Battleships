using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Domain.ValueObjects.GameContext;

namespace Bc.Battleships.Domain.Services
{
    internal static class GameContextProvider
    {
        const int BoardSize = 10;
        internal static GameContext GameContext { get; } = new GameContext(new Length(BoardSize));
    }
}
