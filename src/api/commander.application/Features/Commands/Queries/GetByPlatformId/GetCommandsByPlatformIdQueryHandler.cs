using commander.application.Features.Commands.Dtos;
using commander.domain.Common;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace commander.application.Features.Commands.Queries.GetByPlatformId;

public class GetCommandsByPlatformIdQueryHandler(ICommandRepository commandRepository, ILogger<GetCommandsByPlatformIdQueryHandler> logger)
                : IRequestHandler<GetCommandsByPlatformIdQuery, PaginatedList<CommandsDto>>
{
    private readonly ICommandRepository _commandRepository = commandRepository;
    private readonly ILogger<GetCommandsByPlatformIdQueryHandler> _logger = logger;

    public async Task<PaginatedList<CommandsDto>> Handle(GetCommandsByPlatformIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogDebug("Fetching commands for PlatformId: {PlatformId}, Page: {PageIndex}", request.PlatformId, request.PaginationParams.PageIndex);

        PaginatedList<Command> commands = await _commandRepository.GetCommandsByPlatformIdAsync(request.PlatformId, request.PaginationParams, request.Search, request.SortBy, request.Descending, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Fetched {Count} commands for PlatformId: {PlatformId}", commands.Items.Count, request.PlatformId);
        return new PaginatedList<CommandsDto>(commands.Items.Adapt<List<CommandsDto>>(), commands.PageIndex, commands.PageSize, commands.TotalCount);
    }
}
