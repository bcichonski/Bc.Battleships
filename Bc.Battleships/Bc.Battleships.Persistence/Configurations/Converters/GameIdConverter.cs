using Bc.Battleships.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bc.Battleships.Persistence.Configurations.Converters
{
    internal class GameIdConverter : ValueConverter<GameId, Guid>
    {
        public GameIdConverter() : base(x => x.Value, x => new GameId(x))
        {
        }
    }
}
