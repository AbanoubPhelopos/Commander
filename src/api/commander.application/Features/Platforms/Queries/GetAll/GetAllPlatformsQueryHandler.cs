using commander.application.Features.Platforms.DTOs;
using commander.domain.Common;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace commander.application.Features.Platforms.Queries.GetAll;

public class GetAllPlatformsQueryHandler(IPlatformRepository platformRepository, ILogger<GetAllPlatformsQueryHandler> logger)
    : IRequestHandler<GetAllPlatformsQuery, PaginatedList<PlatformDto>>
{
    private readonly IPlatformRepository _platformRepository = platformRepository;
    private readonly ILogger<GetAllPlatformsQueryHandler> _logger = logger;

    public async Task<PaginatedList<PlatformDto>> Handle(GetAllPlatformsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Fetching all platforms with Page: {PageIndex}, Search: {Search}", request.PaginationParams.PageIndex, request.Search);

        PaginatedList<Platform> platforms = await _platformRepository.GetAllAsync(request.PaginationParams, request.Search, request.SortBy, request.Descending, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Fetched {Count} platforms", platforms.Items.Count);
        return new PaginatedList<PlatformDto>(platforms.Items.Adapt<List<PlatformDto>>(), platforms.PageIndex, platforms.PageSize, platforms.TotalCount);
    }
}
