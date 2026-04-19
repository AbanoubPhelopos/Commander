using commander.domain.Entities;
using commander.domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace commander.infrastructure.Persistence.Repositories;

public class PlatformRepository(AppDbContext context) : IPlatformRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Platform?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Platforms.FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<Platform>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Platforms.ToListAsync(cancellationToken);
    }

    public async Task<Platform> CreateAsync(Platform platform, CancellationToken cancellationToken = default)
    {
        await _context.Platforms.AddAsync(platform, cancellationToken);
        return platform;
    }

    public async Task<Platform?> UpdateAsync(int id, Platform platform, CancellationToken cancellationToken = default)
    {
        Platform? existing = await _context.Platforms.FindAsync([id], cancellationToken);
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
        Platform? entity = await _context.Platforms.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return false;
        }

        _context.Platforms.Remove(entity);
        return true;
    }
}
