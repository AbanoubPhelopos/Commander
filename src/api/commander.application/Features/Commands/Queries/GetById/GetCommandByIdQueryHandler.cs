using commander.application.Features.Commands.Dtos;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;

namespace commander.application.Features.Commands.Queries.GetById;

public class GetCommandByIdQueryHandler(ICommandRepository commandRepository)
                : IRequestHandler<GetCommandByIdQuery, CommandsDto?>
{
    private readonly ICommandRepository _commandRepository = commandRepository;

    public async Task<CommandsDto?> Handle(GetCommandByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Command? existing = await _commandRepository.GetCommandByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (existing is null)
        {
            return null;
        }

        return existing.Adapt<CommandsDto>();
    }
}