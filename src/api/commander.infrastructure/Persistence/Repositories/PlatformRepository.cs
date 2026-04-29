using commander.domain.Common;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace commander.infrastructure.Persistence.Repositories;

public class PlatformRepository(AppDbContext context, ILogger<PlatformRepository> logger) : IPlatformRepository
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<PlatformRepository> _logger = logger;

    public async Task<Platform?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching platform by Id: {PlatformId}", id);
        return await _context.Platforms.FindAsync([id], cancellationToken).ConfigureAwait(false);
    }

    public async Task<string?> GetPlatformNameByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching platform name by Id: {PlatformId}", id);
        return await _context.Platforms.Where(p => p.Id == id).Select(p => p.PlatformName)
                    .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<PaginatedList<Platform>> GetAllAsync(PaginationParams paginationParams, string? search = null, string? sortBy = null, bool descending = false, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching all platforms with Page: {PageIndex}, Search: {Search}, SortBy: {SortBy}", paginationParams.PageIndex, search, sortBy);

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
        _logger.LogInformation("Creating platform with name: {PlatformName}", platform.PlatformName);
        await _context.Platforms.AddAsync(platform, cancellationToken).ConfigureAwait(false);
        return platform;
    }

    public async Task<Platform?> UpdateAsync(int id, Platform platform, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(platform);

        _logger.LogInformation("Updating platform with Id: {PlatformId}", id);

        Platform? existing = await _context.Platforms.FindAsync([id], cancellationToken).ConfigureAwait(false);
        if (existing is null)
        {
            _logger.LogWarning("Platform with Id: {PlatformId} not found for update", id);
            return null;
        }

        existing.PlatformName = platform.PlatformName;
        existing.CreatedAt = platform.CreatedAt;
        return existing;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting platform with Id: {PlatformId}", id);

        Platform? entity = await _context.Platforms.FindAsync([id], cancellationToken).ConfigureAwait(false);
        if (entity is null)
        {
            _logger.LogWarning("Platform with Id: {PlatformId} not found for deletion", id);
            return false;
        }

        _context.Platforms.Remove(entity);
        return true;
    }
}
