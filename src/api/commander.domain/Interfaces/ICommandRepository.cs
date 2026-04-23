using commander.domain.Common;
using commander.domain.Entities;

namespace commander.domain.Interfaces;

public interface ICommandRepository
{
    Task<Command?> GetCommandByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PaginatedList<Command>> GetAllCommandsAsync(PaginationParams paginationParams, string? search = null, string? sortBy = null, bool descending = false, CancellationToken cancellationToken = default);
    Task<PaginatedList<Command>> GetCommandsByPlatformIdAsync(int platformId, PaginationParams paginationParams, string? search = null, string? sortBy = null, bool descending = false, CancellationToken cancellationToken = default);
    Task<Command> CreateCommandAsync(Command command, CancellationToken cancellationToken = default);
    Task<Command?> UpdateCommandAsync(int id, Command command, CancellationToken cancellationToken = default);
    Task<bool> DeleteCommandAsync(int id, CancellationToken cancellationToken = default);
}