using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Persistence.Configurations.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bc.Battleships.Persistence.Configurations
{
    internal class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.Property(x => x.Id)
                .HasConversion<GameIdConverter>();
            builder.HasKey(x => x.Id);
            builder.Ignore(x => x.HumanPlayer);
            builder.Ignore(x => x.GameContext);
            builder.HasIndex(x => x.GameState);

            builder.OwnsOne(x => x.ComputerPlayer, x =>
            {
                x.Ignore(y => y.CurrentHitPoints);
                x.Property(y => y.Id)
                    .HasConversion<PlayerIdConverter>();
                x.WithOwner(y => y.Game);
                x.OwnsMany(y => y.CoordinatesShoot, y =>
                {
                    y.HasKey(z => z.Id);
                    y.Property(z => z.Coordinates)
                        .HasConversion<CoordinatesConverter>();
                });

                x.OwnsMany(y => y.Ships, y =>
                {
                     y.Property(z => z.Id)
                        .HasConversion<ShipIdConverter>();
                     y.HasKey(z => z.Id);
                     y.Ignore(z => z.CurrentHitPoints);
                     y.Property(z => z.Coordinates)
                        .HasConversion<CoordinatesConverter>();
                     y.Property(z => z.Layout);
                     y.Property(z => z.Length)
                        .HasConversion<LengthConverter>();
                     y.Ignore(z => z.CurrentHitPoints);
                     y.WithOwner(z => z.Owner);

                     y.OwnsMany(z => z.Compartments, z =>
                     {
                         z.Property(q => q.Id)
                            .HasConversion<ShipCompartmentIdConverter>();
                         z.HasKey(q => q.Id);
                         z.Property(q => q.Coordinates)
                            .HasConversion<CoordinatesConverter>(); ;
                         z.Property(q => q.State);
                     });
                });
            });
        }
    }
}
