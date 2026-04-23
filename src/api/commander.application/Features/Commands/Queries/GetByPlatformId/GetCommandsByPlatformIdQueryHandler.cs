using commander.application.Features.Commands.Dtos;
using commander.domain.Common;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;

namespace commander.application.Features.Commands.Queries.GetByPlatformId;

public class GetCommandsByPlatformIdQueryHandler(ICommandRepository commandRepository)
                : IRequestHandler<GetCommandsByPlatformIdQuery, PaginatedList<CommandsDto>>
{
    private readonly ICommandRepository _commandRepository = commandRepository;

    public async Task<PaginatedList<CommandsDto>> Handle(GetCommandsByPlatformIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        PaginatedList<Command> commands = await _commandRepository.GetCommandsByPlatformIdAsync(request.PlatformId, request.PaginationParams, cancellationToken).ConfigureAwait(false);
        return new PaginatedList<CommandsDto>(commands.Items.Adapt<List<CommandsDto>>(), commands.PageIndex, commands.PageSize, commands.TotalCount);
    }
}