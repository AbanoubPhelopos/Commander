using commander.domain.Entities;
using commander.domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace commander.infrastructure.Persistence.Repositories;

public class KeyRegistrationRepository(AppDbContext context, ILogger<KeyRegistrationRepository> logger) : IKeyRegistrationRepository
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<KeyRegistrationRepository> _logger = logger;

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Saved {Count} changes to database", result);
        return result >= 0;
    }

    public async Task<KeyRegistration?> GetRegistrationByIndexAsync(string keyIndex, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Looking up key registration by KeyIndex: {KeyIndex}", keyIndex);
        return await _context.KeyRegistrations
            .FirstOrDefaultAsync(k => k.KeyIndex.ToString() == keyIndex, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task CreateRegistrationAsync(KeyRegistration keyRegistration, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(keyRegistration);
        _logger.LogInformation("Creating key registration for UserId: {UserId}", keyRegistration.UserId);
        await _context.KeyRegistrations.AddAsync(keyRegistration, cancellationToken).ConfigureAwait(false);
    }

    public void DeleteRegistration(KeyRegistration keyRegistration)
    {
        ArgumentNullException.ThrowIfNull(keyRegistration);
        _logger.LogInformation("Deleting key registration with KeyIndex: {KeyIndex}", keyRegistration.KeyIndex);
        _context.KeyRegistrations.Remove(keyRegistration);
    }
}
