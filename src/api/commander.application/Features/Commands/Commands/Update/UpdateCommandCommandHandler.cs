using commander.application.Features.Commands.Dtos;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;

namespace commander.application.Features.Commands.Commands.Update;

public class UpdateCommandCommandHandler(IUnitOfWork unitOfWork, ICommandRepository commandRepository)
                : IRequestHandler<UpdateCommandCommand, CommandsDto?>
{
    private readonly ICommandRepository _commandRepository = commandRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CommandsDto?> Handle(UpdateCommandCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Command? existing = await _commandRepository.GetCommandByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (existing is null)
        {
            return null;
        }

        existing.HowTo = request.HowTo;
        existing.CommandLine = request.CommandLine;
        existing.PlatformId = request.PlatformId;

        _ = await _unitOfWork.CommandRepository.UpdateCommandAsync(request.Id, existing, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return existing.Adapt<CommandsDto>();
    }
}