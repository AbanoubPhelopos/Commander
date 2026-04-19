using commander.application.Features.Platforms.DTOs;
using commander.domain.Interfaces;
using Mapster;
using MediatR;

namespace commander.application.Features.Platforms.Queries.GetById;

public class GetPlatformByIdQueryHandler(IPlatformRepository platformRepository)
    : IRequestHandler<GetPlatformByIdQuery, PlatformDto?>
{
    private readonly IPlatformRepository _platformRepository = platformRepository;

    public async Task<PlatformDto?> Handle(GetPlatformByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        commander.domain.Entities.Platform? platform = await _platformRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        return platform?.Adapt<PlatformDto>();
    }
}
