using commander.domain.Common;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace commander.infrastructure.Persistence.Repositories;

public class PlatformRepository(AppDbContext context) : IPlatformRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Platform?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Platforms.FindAsync([id], cancellationToken).ConfigureAwait(false);
    }

    public async Task<string?> GetPlatformNameByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Platforms.Where(p => p.Id == id).Select(p => p.PlatformName)
                    .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<PaginatedList<Platform>> GetAllAsync(PaginationParams paginationParams, string? search = null, string? sortBy = null, bool descending = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Platform> query = _context.Platforms;

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.PlatformName.Contains(search));
        }

        query = ApplySorting(query, sortBy, descending);

        return await query.ToPaginatedListAsync(paginationParams, cancellationToken).ConfigureAwait(false);
    }

    private static IQueryable<Platform> ApplySorting(IQueryable<Platform> query, string? sortBy, bool descending)
    {
        return sortBy?.ToUpperInvariant() switch
        {
            "PLATFORMNAME" => descending ? query.OrderByDescending(p => p.PlatformName) : query.OrderBy(p => p.PlatformName),
            "CREATEDAT" => descending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
            _ => descending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id)
        };
    }

    public async Task<Platform> CreateAsync(Platform platform, CancellationToken cancellationToken = default)
    {
        await _context.Platforms.AddAsync(platform, cancellationToken).ConfigureAwait(false);
        return platform;
    }

    public async Task<Platform?> UpdateAsync(int id, Platform platform, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(platform);

        Platform? existing = await _context.Platforms.FindAsync([id], cancellationToken).ConfigureAwait(false);
        if (existing is null)
        {
            return null;
        }

        existing.PlatformName = platform.PlatformName;
        existing.CreatedAt = platform.CreatedAt;
        return existing;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        Platform? entity = await _context.Platforms.FindAsync([id], cancellationToken).ConfigureAwait(false);
        if (entity is null)
        {
            return false;
        }

        _context.Platforms.Remove(entity);
        return true;
    }
}
