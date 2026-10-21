using Bc.Battleships.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bc.Battleships.Persistence.Configurations.Converters
{
    internal class ShipCompartmentIdConverter : ValueConverter<ShipCompartmentId, Guid>
    {
        public ShipCompartmentIdConverter() : base(x => x.Value, x => new ShipCompartmentId(x))
        {
        }
    }
}
