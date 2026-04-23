using commander.domain.Common;
using commander.domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace commander.infrastructure.Persistence.Repositories;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class
{
    protected AppDbContext Context { get; } = context;

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<T>().FindAsync([id], cancellationToken).ConfigureAwait(false);
    }

    public async Task<PaginatedList<T>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken = default)
    {
        return await Context.Set<T>().ToPaginatedListAsync(paginationParams, cancellationToken).ConfigureAwait(false);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Context.Set<T>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
        return entity;
    }

    public Task<T?> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        Context.Set<T>().Update(entity);
        return Task.FromResult<T?>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        T? entity = await Context.Set<T>().FindAsync([id], cancellationToken).ConfigureAwait(false);
        if (entity is null)
        {
            return false;
        }
        Context.Set<T>().Remove(entity);
        return true;
    }
}
