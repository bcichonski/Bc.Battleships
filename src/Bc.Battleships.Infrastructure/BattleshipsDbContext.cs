using Bc.Battleships.Domain.ValueObjects;
using Bc.Battleships.Infrastructure.Configurations.Converters;
using Microsoft.EntityFrameworkCore;

namespace Bc.Battleships.Infrastructure;

public class BattleshipsDbContext : DbContext
{
    public BattleshipsDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<GameId>().HaveConversion<GuidIdConverter<GameId>>();
        configurationBuilder.Properties<PlayerId>().HaveConversion<GuidIdConverter<PlayerId>>();
        configurationBuilder.Properties<ShipId>().HaveConversion<GuidIdConverter<ShipId>>();
        configurationBuilder.Properties<ShipCompartmentId>().HaveConversion<GuidIdConverter<ShipCompartmentId>>();
        configurationBuilder.Properties<Coordinates>().HaveConversion<CoordinatesConverter>();
        configurationBuilder.Properties<Length>().HaveConversion<LengthConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
