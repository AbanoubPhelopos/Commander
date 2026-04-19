using System.Collections.Generic;
using commander.application.Features.Commands.Dtos;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;

namespace commander.application.Features.Commands.Queries.GetAll;

public class GetAllCommandsQueryHandler(ICommandRepository commandRepository) : IRequestHandler<GetAllCommandsQuery, IEnumerable<CommandsDto>>
{
    private readonly ICommandRepository _commandRepository = commandRepository;

    public async Task<IEnumerable<CommandsDto>> Handle(GetAllCommandsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Command> commands = await _commandRepository.GetAllCommandsAsync(cancellationToken).ConfigureAwait(false);
        return commands.Adapt<IEnumerable<CommandsDto>>();
    }
}