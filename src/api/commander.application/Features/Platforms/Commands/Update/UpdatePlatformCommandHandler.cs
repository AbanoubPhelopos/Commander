using commander.application.Features.Platforms.DTOs;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;

namespace commander.application.Features.Platforms.Commands.Update;

public class UpdatePlatformCommandHandler(IPlatformRepository platformRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePlatformCommand, PlatformDto?>
{
    private readonly IPlatformRepository _platformRepository = platformRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PlatformDto?> Handle(UpdatePlatformCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Platform? existing = await _platformRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (existing is null)
        {
            return null;
        }

        existing.PlatformName = request.PlatformName;
        existing.CreatedAt = request.CreatedAt;
        _ = await _unitOfWork.PlatformRepository.UpdateAsync(request.Id, existing, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return existing.Adapt<PlatformDto>();
    }
}
