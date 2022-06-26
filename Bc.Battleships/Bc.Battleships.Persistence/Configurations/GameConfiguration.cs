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

            var ownedComputerPlayer = builder.OwnsOne(x => x.ComputerPlayer);
            builder.Ignore(x => x.GameContext);
            builder.HasIndex(x => x.GameState);

            ownedComputerPlayer.Ignore(x => x.CurrentHitPoints);
            ownedComputerPlayer.Property(x => x.Id)
                .HasConversion<PlayerIdConverter>();

            var ownedShips = ownedComputerPlayer.OwnsMany(x => x.Ships);
            ownedShips.Property(x => x.Id)
                .HasConversion<ShipIdConverter>();
            ownedShips.HasKey(x => x.Id);
            ownedShips
                .Ignore(x => x.CurrentHitPoints);
            
            var ownedShipsCompartments = ownedShips.OwnsMany(x => x.Compartments);
            ownedShipsCompartments.Property(x => x.Id)
                .HasConversion<ShipCompartmentIdConverter>();
            ownedShipsCompartments.HasKey(x => x.Id);
        }
    }
}
