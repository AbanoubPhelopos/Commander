using commander.application.Features.Commands.Dtos;
using commander.domain.Common;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace commander.application.Features.Commands.Queries.GetAll;

public class GetAllCommandsQueryHandler(ICommandRepository commandRepository, ILogger<GetAllCommandsQueryHandler> logger) : IRequestHandler<GetAllCommandsQuery, PaginatedList<CommandsDto>>
{
    private readonly ICommandRepository _commandRepository = commandRepository;
    private readonly ILogger<GetAllCommandsQueryHandler> _logger = logger;

    public async Task<PaginatedList<CommandsDto>> Handle(GetAllCommandsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Fetching all commands with Page: {PageIndex}, Search: {Search}", request.PaginationParams.PageIndex, request.Search);

        PaginatedList<Command> commands = await _commandRepository.GetAllCommandsAsync(request.PaginationParams, request.Search, request.SortBy, request.Descending, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Fetched {Count} commands", commands.Items.Count);
        return new PaginatedList<CommandsDto>(commands.Items.Adapt<List<CommandsDto>>(), commands.PageIndex, commands.PageSize, commands.TotalCount);
    }
}
