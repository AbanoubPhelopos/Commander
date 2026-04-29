using commander.application.Features.Platforms.DTOs;
using commander.domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace commander.application.Features.Platforms.Queries.GetById;

public class GetPlatformByIdQueryHandler(IPlatformRepository platformRepository, ILogger<GetPlatformByIdQueryHandler> logger)
    : IRequestHandler<GetPlatformByIdQuery, PlatformDto?>
{
    private readonly IPlatformRepository _platformRepository = platformRepository;
    private readonly ILogger<GetPlatformByIdQueryHandler> _logger = logger;

    public async Task<PlatformDto?> Handle(GetPlatformByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogDebug("Fetching platform with Id: {PlatformId}", request.Id);

        commander.domain.Entities.Platform? platform = await _platformRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (platform is null)
        {
            _logger.LogWarning("Platform with Id: {PlatformId} not found", request.Id);
        }

        return platform?.Adapt<PlatformDto>();
    }
}
