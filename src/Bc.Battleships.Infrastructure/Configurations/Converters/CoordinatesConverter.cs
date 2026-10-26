using Bc.Battleships.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bc.Battleships.Infrastructure.Configurations.Converters;

internal class CoordinatesConverter : ValueConverter<Coordinates, string>
{
    public CoordinatesConverter() : base(x => x.ToString(), x => Coordinates.Parse(x))
    { }
}
