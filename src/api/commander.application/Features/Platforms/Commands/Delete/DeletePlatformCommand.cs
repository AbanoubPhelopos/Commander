using commander.domain.Entities;
using commander.domain.Interfaces;

namespace commander.application.Features.Platforms.Commands.Delete;

public record DeletePlatformCommand(int Id) : MediatR.IRequest<bool>;

public class DeletePlatformCommandHandler(IUnitOfWork unitOfWork)
    : MediatR.IRequestHandler<DeletePlatformCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeletePlatformCommand request, CancellationToken cancellationToken)
    {
        bool deleted = await _unitOfWork.Repository<Platform>().DeleteAsync(request.Id, cancellationToken);
        if (deleted)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        return deleted;
    }
}