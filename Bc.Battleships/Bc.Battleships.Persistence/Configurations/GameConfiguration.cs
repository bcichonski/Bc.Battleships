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
            builder.OwnsOne(x => x.ComputerPlayer)
                .Property(x => x.Id)
                    .HasConversion<PlayerIdConverter>();
            builder.Ignore(x => x.GameContext);

            builder.HasIndex(x => x.GameState);
        }
    }
}
