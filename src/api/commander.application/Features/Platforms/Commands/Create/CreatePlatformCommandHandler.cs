using commander.application.Features.Platforms.DTOs;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;

namespace commander.application.Features.Platforms.Commands.Create;

public class CreatePlatformCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePlatformCommand, PlatformDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PlatformDto> Handle(CreatePlatformCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Platform platform = new() { PlatformName = request.PlatformName, CreatedAt = DateTime.UtcNow };
        await _unitOfWork.Repository<Platform>().AddAsync(platform, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return platform.Adapt<PlatformDto>();
    }
}
