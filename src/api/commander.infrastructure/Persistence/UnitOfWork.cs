using commander.domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace commander.infrastructure.Persistence;

public class UnitOfWork(AppDbContext context, IPlatformRepository platformRepository, ICommandRepository commandRepository, ILogger<UnitOfWork> logger) : IUnitOfWork
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<UnitOfWork> _logger = logger;
    private IDbContextTransaction? _transaction;

    public IPlatformRepository PlatformRepository { get; } = platformRepository;
    public ICommandRepository CommandRepository { get; } = commandRepository;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Saved {Count} changes to database", result);
        return result;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Beginning database transaction");
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            _logger.LogInformation("Committing database transaction");
            await _transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            await _transaction.DisposeAsync().ConfigureAwait(false);
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            _logger.LogWarning("Rolling back database transaction");
            await _transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            await _transaction.DisposeAsync().ConfigureAwait(false);
            _transaction = null;
        }
    }
}
