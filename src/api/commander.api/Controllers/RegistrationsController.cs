using System.Security.Cryptography;
using commander.application.Features.KeyRegistrations.DTOs;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Api.Controllers;

#pragma warning disable CA1515
[ApiController]
[Route("api/[controller]")]
public class RegistrationsController(IKeyRegistrationRepository regoRepo, ILogger<RegistrationsController> logger) : ControllerBase
#pragma warning restore CA1515
{
    private readonly IKeyRegistrationRepository _regoRepo = regoRepo;
    private readonly ILogger<RegistrationsController> _logger = logger;

    [HttpPost]
    public async Task<ActionResult> RegisterKey([FromBody] KeyRegistrationCreateDto keyRegistrationCreateDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registering new API key");

        var keyIndex = Guid.NewGuid();

        byte[] secretBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(secretBytes);
        }

        string secret = Convert.ToBase64String(secretBytes).TrimEnd('=');

        string fullKey = keyIndex.ToString() + secret;

        byte[] saltBytes = new byte[16];
        using (var rng2 = RandomNumberGenerator.Create())
        {
            rng2.GetBytes(saltBytes);
        }

        Guid salt = new(saltBytes);

        byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
            fullKey,
            saltBytes,
            100000,
            HashAlgorithmName.SHA256,
            32);

        string keyHash = Convert.ToBase64String(hashBytes);

        KeyRegistration keyRego = new()
        {
            KeyIndex = keyIndex,
            Salt = salt,
            KeyHash = keyHash,
            Description = keyRegistrationCreateDto.Description,
            UserId = keyRegistrationCreateDto.UserId
        };

        await _regoRepo.CreateRegistrationAsync(keyRego, cancellationToken).ConfigureAwait(false);
        await _regoRepo.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Successfully registered API key with KeyIndex: {KeyIndex}", keyIndex);

        return Ok(new
        {
            apiKey = fullKey,
            message = "Store this key securely. It will not be shown again."
        });
    }
}
