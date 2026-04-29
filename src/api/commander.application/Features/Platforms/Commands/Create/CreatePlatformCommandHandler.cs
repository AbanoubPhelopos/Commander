using commander.application.Features.Platforms.DTOs;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace commander.application.Features.Platforms.Commands.Create;

public class CreatePlatformCommandHandler(IUnitOfWork unitOfWork, ILogger<CreatePlatformCommandHandler> logger)
    : IRequestHandler<CreatePlatformCommand, PlatformDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CreatePlatformCommandHandler> _logger = logger;

    public async Task<PlatformDto> Handle(CreatePlatformCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Creating platform with name: {PlatformName}", request.PlatformName);

        Platform platform = new() { PlatformName = request.PlatformName, CreatedAt = DateTime.UtcNow };
        await _unitOfWork.PlatformRepository.CreateAsync(platform, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Platform created with Id: {PlatformId}", platform.Id);
        return platform.Adapt<PlatformDto>();
    }
}
