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
            builder.OwnsMany(x => x.Players)
                .Property(x => x.Id)
                    .HasConversion<PlayerIdConverter>();

            builder.HasIndex(x => x.GameState);
        }
    }
}
