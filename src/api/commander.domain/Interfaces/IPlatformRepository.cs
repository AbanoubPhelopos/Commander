using commander.domain.Entities;

namespace commander.domain.Interfaces;

public interface IPlatformRepository
{
    Task<Platform?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Platform>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Platform> CreateAsync(Platform platform, CancellationToken cancellationToken = default);
    Task<Platform?> UpdateAsync(int id, Platform platform, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}