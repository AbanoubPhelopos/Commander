using commander.domain.Common;
using commander.domain.Entities;

namespace commander.domain.Interfaces;

public interface IPlatformRepository
{
    Task<Platform?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PaginatedList<Platform>> GetAllAsync(PaginationParams paginationParams, string? search = null, string? sortBy = null, bool descending = false, CancellationToken cancellationToken = default);
    Task<Platform> CreateAsync(Platform platform, CancellationToken cancellationToken = default);
    Task<Platform?> UpdateAsync(int id, Platform platform, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}