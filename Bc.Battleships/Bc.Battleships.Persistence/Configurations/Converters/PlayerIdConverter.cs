using Bc.Battleships.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bc.Battleships.Persistence.Configurations.Converters
{
    internal class PlayerIdConverter : ValueConverter<PlayerId, Guid>
    {
        public PlayerIdConverter() : base(x => x.Value, x => new PlayerId(x))
        {
        }
    }
}
