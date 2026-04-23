using commander.domain.Common;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace commander.infrastructure.Persistence.Repositories;

public class CommandRepository(AppDbContext context) : ICommandRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Command?> GetCommandByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Commands
            .Include(c => c.Platform)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PaginatedList<Command>> GetAllCommandsAsync(PaginationParams paginationParams, string? search = null, string? sortBy = null, bool descending = false, CancellationToken cancellationToken = default)
    {
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
        await _context.Commands.AddAsync(command, cancellationToken).ConfigureAwait(false);
        return command;
    }

    public async Task<Command?> UpdateCommandAsync(int id, Command command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        Command? existing = await _context.Commands.FindAsync([id], cancellationToken).ConfigureAwait(false);
        if (existing is null)
        {
            return null;
        }

        existing.PlatformId = command.PlatformId;
        existing.HowTo = command.HowTo;
        existing.CommandLine = command.CommandLine;
        return existing;
    }

    public async Task<bool> DeleteCommandAsync(int id, CancellationToken cancellationToken = default)
    {
        Command? entity = await _context.Commands.FindAsync([id], cancellationToken).ConfigureAwait(false);
        if (entity is null)
        {
            return false;
        }

        _context.Commands.Remove(entity);
        return true;
    }
}