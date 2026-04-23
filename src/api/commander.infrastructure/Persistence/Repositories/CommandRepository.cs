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

    public async Task<PaginatedList<Command>> GetAllCommandsAsync(PaginationParams paginationParams, CancellationToken cancellationToken = default)
    {
        return await _context.Commands
            .Include(c => c.Platform)
            .ToPaginatedListAsync(paginationParams, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PaginatedList<Command>> GetCommandsByPlatformIdAsync(int platformId, PaginationParams paginationParams, CancellationToken cancellationToken = default)
    {
        return await _context.Commands
            .Include(c => c.Platform)
            .Where(c => c.PlatformId == platformId)
            .ToPaginatedListAsync(paginationParams, cancellationToken).ConfigureAwait(false);
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