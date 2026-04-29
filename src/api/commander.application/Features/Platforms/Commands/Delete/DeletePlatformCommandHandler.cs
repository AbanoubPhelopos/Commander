using commander.domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace commander.application.Features.Platforms.Commands.Delete;

public class DeletePlatformCommandHandler(IPlatformRepository platformRepository, IUnitOfWork unitOfWork, ILogger<DeletePlatformCommandHandler> logger)
    : IRequestHandler<DeletePlatformCommand, bool>
{
    private readonly IPlatformRepository _platformRepository = platformRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<DeletePlatformCommandHandler> _logger = logger;

    public async Task<bool> Handle(DeletePlatformCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Deleting platform with Id: {PlatformId}", request.Id);

        bool exists = await _platformRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false) is not null;
        if (!exists)
        {
            _logger.LogWarning("Platform with Id: {PlatformId} not found for deletion", request.Id);
            return false;
        }

        bool deleted = await _unitOfWork.PlatformRepository.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (deleted)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Platform with Id: {PlatformId} deleted successfully", request.Id);
        }

        return deleted;
    }
}
