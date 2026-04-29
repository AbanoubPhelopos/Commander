using commander.application.Features.Commands.Dtos;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace commander.application.Features.Commands.Queries.GetById;

public class GetCommandByIdQueryHandler(ICommandRepository commandRepository, ILogger<GetCommandByIdQueryHandler> logger)
                : IRequestHandler<GetCommandByIdQuery, CommandsDto?>
{
    private readonly ICommandRepository _commandRepository = commandRepository;
    private readonly ILogger<GetCommandByIdQueryHandler> _logger = logger;

    public async Task<CommandsDto?> Handle(GetCommandByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogDebug("Fetching command with Id: {CommandId}", request.Id);

        Command? existing = await _commandRepository.GetCommandByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (existing is null)
        {
            _logger.LogWarning("Command with Id: {CommandId} not found", request.Id);
            return null;
        }

        return existing.Adapt<CommandsDto>();
    }
}
