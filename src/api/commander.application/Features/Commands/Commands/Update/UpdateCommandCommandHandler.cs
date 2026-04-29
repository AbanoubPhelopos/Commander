using commander.application.Features.Commands.Dtos;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace commander.application.Features.Commands.Commands.Update;

public class UpdateCommandCommandHandler(IUnitOfWork unitOfWork, ICommandRepository commandRepository, ILogger<UpdateCommandCommandHandler> logger)
                : IRequestHandler<UpdateCommandCommand, CommandsDto?>
{
    private readonly ICommandRepository _commandRepository = commandRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<UpdateCommandCommandHandler> _logger = logger;

    public async Task<CommandsDto?> Handle(UpdateCommandCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Updating command with Id: {CommandId}", request.Id);

        Command? existing = await _commandRepository.GetCommandByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (existing is null)
        {
            _logger.LogWarning("Command with Id: {CommandId} not found for update", request.Id);
            return null;
        }

        existing.HowTo = request.HowTo;
        existing.CommandLine = request.CommandLine;
        existing.PlatformId = request.PlatformId;

        _ = await _unitOfWork.CommandRepository.UpdateCommandAsync(request.Id, existing, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Command with Id: {CommandId} updated successfully", request.Id);
        return existing.Adapt<CommandsDto>();
    }
}
