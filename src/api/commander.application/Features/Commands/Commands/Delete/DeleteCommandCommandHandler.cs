using commander.domain.Entities;
using commander.domain.Interfaces;
using MediatR;

namespace commander.application.Features.Commands.Commands.Delete;

public class DeleteCommandCommandHandler(IUnitOfWork unitOfWork, ICommandRepository commandRepository)
                : IRequestHandler<DeleteCommandCommand, bool>
{
    private readonly ICommandRepository _commandRepository = commandRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteCommandCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        bool exists = await _commandRepository.GetCommandByIdAsync(request.Id, cancellationToken).ConfigureAwait(false) is not null;
        if (!exists)
        {
            return false;
        }

        bool deleted = await _unitOfWork.Repository<Command>().DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (deleted)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        return deleted;
    }
}