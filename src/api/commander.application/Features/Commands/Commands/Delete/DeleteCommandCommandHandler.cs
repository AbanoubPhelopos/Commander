using commander.domain.Entities;
using commander.domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace commander.application.Features.Commands.Commands.Delete;

public class DeleteCommandCommandHandler(IUnitOfWork unitOfWork, ICommandRepository commandRepository, ILogger<DeleteCommandCommandHandler> logger)
                : IRequestHandler<DeleteCommandCommand, bool>
{
    private readonly ICommandRepository _commandRepository = commandRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<DeleteCommandCommandHandler> _logger = logger;

    public async Task<bool> Handle(DeleteCommandCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Deleting command with Id: {CommandId}", request.Id);

        bool exists = await _commandRepository.GetCommandByIdAsync(request.Id, cancellationToken).ConfigureAwait(false) is not null;
        if (!exists)
        {
            _logger.LogWarning("Command with Id: {CommandId} not found for deletion", request.Id);
            return false;
        }

        bool deleted = await _unitOfWork.CommandRepository.DeleteCommandAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (deleted)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Command with Id: {CommandId} deleted successfully", request.Id);
        }

        return deleted;
    }
}
