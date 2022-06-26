using Bc.Battleships.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bc.Battleships.Persistence.Configurations.Converters
{
    internal class ShipIdConverter : ValueConverter<ShipId, Guid>
    {
        public ShipIdConverter() : base(x => x.Value, x => new ShipId(x))
        {
        }
    }
}
