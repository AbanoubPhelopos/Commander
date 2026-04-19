using commander.application.Features.Commands.Dtos;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;

namespace commander.application.Features.Commands.Queries.GetByPlatformId;

public class GetCommandsByPlatformIdQueryHandler(ICommandRepository commandRepository)
                : IRequestHandler<GetCommandsByPlatformIdQuery, IEnumerable<CommandsDto>>
{
    private readonly ICommandRepository _commandRepository = commandRepository;

    public async Task<IEnumerable<CommandsDto>> Handle(GetCommandsByPlatformIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        IEnumerable<Command> commands = await _commandRepository.GetCommandsByPlatformIdAsync(request.PlatformId, cancellationToken).ConfigureAwait(false);
        return commands.Adapt<IEnumerable<CommandsDto>>();
    }
}