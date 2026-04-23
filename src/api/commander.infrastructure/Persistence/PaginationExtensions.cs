using commander.domain.Common;
using Microsoft.EntityFrameworkCore;

namespace commander.infrastructure.Persistence;

public static class PaginationExtensions
{
    public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> source, PaginationParams paginationParams, CancellationToken cancellationToken = default)
    {
        int count = await source.CountAsync(cancellationToken).ConfigureAwait(false);

        List<T> items = await source
            .Skip((paginationParams.PageIndex - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return new PaginatedList<T>(items, paginationParams.PageIndex, paginationParams.PageSize, count);
    }
}
