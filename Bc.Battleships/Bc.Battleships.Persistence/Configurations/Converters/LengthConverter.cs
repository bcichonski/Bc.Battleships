using Bc.Battleships.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bc.Battleships.Persistence.Configurations.Converters
{
    public class LengthConverter : ValueConverter<Length, int>
    {
        public LengthConverter() : base(x => x.Value, x => new Length(x))
        { }
    }
}
