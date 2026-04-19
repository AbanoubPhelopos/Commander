using commander.domain.Interfaces;
using MediatR;

namespace commander.application.Features.Platforms.Commands.Delete;

public class DeletePlatformCommandHandler(IPlatformRepository platformRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeletePlatformCommand, bool>
{
    private readonly IPlatformRepository _platformRepository = platformRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeletePlatformCommand request, CancellationToken cancellationToken)
    {
        bool exists = await _platformRepository.GetByIdAsync(request.Id, cancellationToken) is not null;
        if (!exists)
        {
            return false;
        }

        bool deleted = await _unitOfWork.Repository<commander.domain.Entities.Platform>().DeleteAsync(request.Id, cancellationToken);
        if (deleted)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        return deleted;
    }
}
