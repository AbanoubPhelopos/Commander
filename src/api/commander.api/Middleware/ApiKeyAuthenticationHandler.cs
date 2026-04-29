using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Options;

namespace commander.api.Middleware;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string ApiKeyHeaderName = "x-api-key";

    private readonly IKeyRegistrationRepository _regRepo;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IKeyRegistrationRepository regRepo)
        : base(options, logger, encoder)
    {
        _regRepo = regRepo;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out StringValues apiKeyHeaderValues))
        {
            Logger.LogWarning("API key header missing from request");
            return AuthenticateResult.NoResult();
        }

        string providedApiKey = apiKeyHeaderValues.FirstOrDefault();

        if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(providedApiKey))
        {
            Logger.LogWarning("Empty API key provided");
            return AuthenticateResult.NoResult();
        }

        KeyRegistration? keyRego = await ValidateKey(providedApiKey).ConfigureAwait(false);
        if (keyRego is not null)
        {
            Logger.LogInformation("API key authentication successful for user {UserId}", keyRego.UserId);
            Claim[] claims =
            {
                new Claim(ClaimTypes.Name, "ApiKeyUser"),
                new Claim(ClaimTypes.NameIdentifier, keyRego.UserId),
                new Claim("KeyIndex", keyRego.KeyIndex.ToString())
            };
            ClaimsIdentity identity = new(claims, Scheme.Name);
            ClaimsPrincipal principal = new(identity);
            AuthenticationTicket ticket = new(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        Logger.LogWarning("Invalid API key provided");
        return AuthenticateResult.Fail("Invalid API Key Provided");
    }

    private async Task<KeyRegistration?> ValidateKey(string providedApiKey)
    {
        string extractedIndex = providedApiKey[..36];
        KeyRegistration? keyRego = await _regRepo.GetRegistrationByIndexAsync(extractedIndex).ConfigureAwait(false);

        if (keyRego is null)
        {
            return null;
        }

        byte[] saltBytes = keyRego.Salt.ToByteArray();

        byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
            providedApiKey,
            saltBytes,
            100000,
            HashAlgorithmName.SHA256,
            32);

        string generatedHash = Convert.ToBase64String(hashBytes);

        byte[] storedHashBytes = Convert.FromBase64String(keyRego.KeyHash);
        byte[] generatedHashBytes = Convert.FromBase64String(generatedHash);

        bool isValid = CryptographicOperations.FixedTimeEquals(storedHashBytes, generatedHashBytes);
        return isValid ? keyRego : null;
    }
}
