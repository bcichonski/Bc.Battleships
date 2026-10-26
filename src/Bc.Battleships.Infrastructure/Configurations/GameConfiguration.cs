using Bc.Battleships.Domain.AggregateRoots.Games;
using Bc.Battleships.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bc.Battleships.Infrastructure.Configurations;

internal class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Ignore(x => x.HumanPlayer);
        builder.HasIndex(x => x.GameState);

        builder.OwnsOne(x => x.ComputerPlayer, x =>
        {
            x.Ignore(y => y.CurrentHitPoints);
            x.WithOwner();
            x.OwnsMany(y => y.CoordinatesShoot, y =>
            {
                y.HasKey(z => z.Id);
                y.Property(z => z.Coordinates);
            });

            x.OwnsMany(y => y.Ships, y =>
            {
                 y.HasKey(z => z.Id);
                 y.Ignore(z => z.CurrentHitPoints);
                 y.Property(z => z.Coordinates);
                 y.Property(z => z.Layout);
                 y.Property(z => z.Length);
                 y.Ignore(z => z.CurrentHitPoints);
                 y.Property<GameId>("OwnerGameId").IsRequired();
                 y.WithOwner().HasForeignKey("OwnerGameId");

                 y.OwnsMany(z => z.Compartments, z =>
                 {
                     z.HasKey(q => q.Id);
                     z.Property(q => q.Coordinates);
                     z.Property(q => q.State);
                 });
            });
        });
    }
}
