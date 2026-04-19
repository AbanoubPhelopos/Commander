using commander.application.Features.Platforms.DTOs;
using commander.domain.Entities;
using commander.domain.Interfaces;
using MediatR;

namespace commander.application.Features.Platforms.Commands.Update;

public record UpdatePlatformCommand(int Id, string PlatformName, DateTime CreatedAt) : IRequest<PlatformDto?>;

public class UpdatePlatformCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePlatformCommand, PlatformDto?>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PlatformDto?> Handle(UpdatePlatformCommand request, CancellationToken cancellationToken)
    {
        Platform? existing = await _unitOfWork.Repository<Platform>().GetByIdAsync(request.Id, cancellationToken);
        if (existing is null)
        {
            return null;
        }
        existing.PlatformName = request.PlatformName;
        existing.CreatedAt = request.CreatedAt;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new PlatformDto(existing.Id, existing.PlatformName, existing.CreatedAt);
    }
}