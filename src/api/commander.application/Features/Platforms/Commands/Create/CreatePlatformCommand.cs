using commander.application.Features.Platforms.DTOs;
using commander.domain.Entities;
using commander.domain.Interfaces;
using MediatR;

namespace commander.application.Features.Platforms.Commands.Create;

public record CreatePlatformCommand(string PlatformName) : IRequest<PlatformDto>;

public class CreatePlatformCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePlatformCommand, PlatformDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PlatformDto> Handle(CreatePlatformCommand request, CancellationToken cancellationToken)
    {
        Platform platform = new() { PlatformName = request.PlatformName };
        await _unitOfWork.Repository<Platform>().AddAsync(platform, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new PlatformDto(platform.Id, platform.PlatformName);
    }
}