using commander.application.Features.Platforms.DTOs;
using commander.domain.Entities;
using commander.domain.Interfaces;
using MediatR;

namespace commander.application.Features.Platforms.Commands.Update;

public class UpdatePlatformCommandHandler(IPlatformRepository platformRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePlatformCommand, PlatformDto?>
{
    private readonly IPlatformRepository _platformRepository = platformRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PlatformDto?> Handle(UpdatePlatformCommand request, CancellationToken cancellationToken)
    {
        Platform? existing = await _platformRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        existing.PlatformName = request.PlatformName;
        existing.CreatedAt = request.CreatedAt;
        _ = await _unitOfWork.Repository<Platform>().UpdateAsync(request.Id, existing, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new PlatformDto(existing.Id, existing.PlatformName, existing.CreatedAt);
    }
}
