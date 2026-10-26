using Bc.Battleships.Domain.Repositories;

namespace Bc.Battleships.Infrastructure.Repositories;

public class UnitOfWork(BattleshipsDbContext battleshipsDbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await battleshipsDbContext.SaveChangesAsync(cancellationToken);
}
