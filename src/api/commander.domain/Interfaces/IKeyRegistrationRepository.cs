using commander.domain.Entities;

namespace commander.domain.Interfaces;

public interface IKeyRegistrationRepository
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<KeyRegistration?> GetRegistrationByIndexAsync(string keyIndex, CancellationToken cancellationToken = default);
    Task CreateRegistrationAsync(KeyRegistration keyRegistration, CancellationToken cancellationToken = default);
    void DeleteRegistration(KeyRegistration keyRegistration);
}
