using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace WebApi.Endpoints.IngestAppointment;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(
        TEntity entity,
        CancellationToken cancellationToken = default)
        where TEntity : class;
}