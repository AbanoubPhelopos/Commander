using commander.application.Features.Platforms.DTOs;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;

namespace commander.application.Features.Platforms.Queries.GetAll;

public class GetAllPlatformsQueryHandler(IPlatformRepository platformRepository)
    : IRequestHandler<GetAllPlatformsQuery, IEnumerable<PlatformDto>>
{
    private readonly IPlatformRepository _platformRepository = platformRepository;

    public async Task<IEnumerable<PlatformDto>> Handle(GetAllPlatformsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Platform> platforms = await _platformRepository.GetAllAsync(cancellationToken);
        return platforms.Adapt<IEnumerable<PlatformDto>>();
    }
}
