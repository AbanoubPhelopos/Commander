using commander.application.Features.Platforms.DTOs;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace commander.application.Features.Platforms.Commands.Update;

public class UpdatePlatformCommandHandler(IPlatformRepository platformRepository, IUnitOfWork unitOfWork, ILogger<UpdatePlatformCommandHandler> logger)
    : IRequestHandler<UpdatePlatformCommand, PlatformDto?>
{
    private readonly IPlatformRepository _platformRepository = platformRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<UpdatePlatformCommandHandler> _logger = logger;

    public async Task<PlatformDto?> Handle(UpdatePlatformCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Updating platform with Id: {PlatformId}", request.Id);

        Platform? existing = await _platformRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (existing is null)
        {
            _logger.LogWarning("Platform with Id: {PlatformId} not found for update", request.Id);
            return null;
        }

        existing.PlatformName = request.PlatformName;
        existing.CreatedAt = request.CreatedAt;
        _ = await _unitOfWork.PlatformRepository.UpdateAsync(request.Id, existing, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Platform with Id: {PlatformId} updated successfully", request.Id);
        return existing.Adapt<PlatformDto>();
    }
}
