using commander.domain.Common;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace commander.infrastructure.Persistence.Repositories;

public class CommandRepository(AppDbContext context, ILogger<CommandRepository> logger) : ICommandRepository
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<CommandRepository> _logger = logger;

    public async Task<Command?> GetCommandByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching command by Id: {CommandId}", id);
        return await _context.Commands
            .Include(c => c.Platform)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PaginatedList<Command>> GetAllCommandsAsync(PaginationParams paginationParams, string? search = null, string? sortBy = null, bool descending = false, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching all commands with Page: {PageIndex}, Search: {Search}, SortBy: {SortBy}", paginationParams.PageIndex, search, sortBy);

        IQueryable<Command> query = _context.Commands.Include(c => c.Platform);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.HowTo!.Contains(search) || c.CommandLine!.Contains(search));
        }

        query = ApplySorting(query, sortBy, descending);

        return await query.ToPaginatedListAsync(paginationParams, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PaginatedList<Command>> GetCommandsByPlatformIdAsync(int platformId, PaginationParams paginationParams, string? search = null, string? sortBy = null, bool descending = false, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching commands for PlatformId: {PlatformId}, Page: {PageIndex}", platformId, paginationParams.PageIndex);

        IQueryable<Command> query = _context.Commands
            .Include(c => c.Platform)
            .Where(c => c.PlatformId == platformId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.HowTo!.Contains(search) || c.CommandLine!.Contains(search));
        }

        query = ApplySorting(query, sortBy, descending);

        return await query.ToPaginatedListAsync(paginationParams, cancellationToken).ConfigureAwait(false);
    }

    private static IQueryable<Command> ApplySorting(IQueryable<Command> query, string? sortBy, bool descending)
    {
        return sortBy?.ToUpperInvariant() switch
        {
            "HOWTO" => descending ? query.OrderByDescending(c => c.HowTo) : query.OrderBy(c => c.HowTo),
            "COMMANDLINE" => descending ? query.OrderByDescending(c => c.CommandLine) : query.OrderBy(c => c.CommandLine),
            "PLATFORMID" => descending ? query.OrderByDescending(c => c.PlatformId) : query.OrderBy(c => c.PlatformId),
            "CREATEDAT" => descending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
            _ => descending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id)
        };
    }

    public async Task<Command> CreateCommandAsync(Command command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating command with HowTo: {HowTo}, PlatformId: {PlatformId}", command.HowTo, command.PlatformId);
        await _context.Commands.AddAsync(command, cancellationToken).ConfigureAwait(false);
        return command;
    }

    public async Task<Command?> UpdateCommandAsync(int id, Command command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        _logger.LogInformation("Updating command with Id: {CommandId}", id);

        Command? existing = await _context.Commands.FindAsync([id], cancellationToken).ConfigureAwait(false);
        if (existing is null)
        {
            _logger.LogWarning("Command with Id: {CommandId} not found for update", id);
            return null;
        }

        existing.PlatformId = command.PlatformId;
        existing.HowTo = command.HowTo;
        existing.CommandLine = command.CommandLine;
        return existing;
    }

    public async Task<bool> DeleteCommandAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting command with Id: {CommandId}", id);

        Command? entity = await _context.Commands.FindAsync([id], cancellationToken).ConfigureAwait(false);
        if (entity is null)
        {
            _logger.LogWarning("Command with Id: {CommandId} not found for deletion", id);
            return false;
        }

        _context.Commands.Remove(entity);
        return true;
    }
}
