using commander.application.Features.Platforms.DTOs;
using commander.domain.Common;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;

namespace commander.application.Features.Platforms.Queries.GetAll;

public class GetAllPlatformsQueryHandler(IPlatformRepository platformRepository)
    : IRequestHandler<GetAllPlatformsQuery, PaginatedList<PlatformDto>>
{
    private readonly IPlatformRepository _platformRepository = platformRepository;

    public async Task<PaginatedList<PlatformDto>> Handle(GetAllPlatformsQuery request, CancellationToken cancellationToken)
    {
        PaginatedList<Platform> platforms = await _platformRepository.GetAllAsync(request.PaginationParams, request.Search, request.SortBy, request.Descending, cancellationToken).ConfigureAwait(false);
        return new PaginatedList<PlatformDto>(platforms.Items.Adapt<List<PlatformDto>>(), platforms.PageIndex, platforms.PageSize, platforms.TotalCount);
    }
}
