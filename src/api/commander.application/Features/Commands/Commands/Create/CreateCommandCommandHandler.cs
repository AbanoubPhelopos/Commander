using commander.application.Features.Commands.Dtos;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace commander.application.Features.Commands.Commands.Create;

public class CreateCommandCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateCommandCommandHandler> logger)
                : IRequestHandler<CreateCommandCommand, CommandsDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CreateCommandCommandHandler> _logger = logger;

    public async Task<CommandsDto> Handle(CreateCommandCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Creating command with HowTo: {HowTo}, PlatformId: {PlatformId}", request.HowTo, request.PlatformId);

        Command command = new()
        {
            HowTo = request.HowTo,
            CommandLine = request.CommandLine,
            PlatformId = request.PlatformId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.CommandRepository.CreateCommandAsync(command, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Command created with Id: {CommandId}", command.Id);
        return command.Adapt<CommandsDto>();
    }
}
