using commander.application.Features.Platforms.DTOs;
using commander.domain.Entities;
using commander.domain.Interfaces;

namespace commander.application.Features.Platforms.Queries.GetById;

public record GetPlatformByIdQuery(int Id) : MediatR.IRequest<PlatformDto?>;

public class GetPlatformByIdQueryHandler(IUnitOfWork unitOfWork)
    : MediatR.IRequestHandler<GetPlatformByIdQuery, PlatformDto?>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PlatformDto?> Handle(GetPlatformByIdQuery request, CancellationToken cancellationToken)
    {
        Platform? platform = await _unitOfWork.Repository<Platform>().GetByIdAsync(request.Id, cancellationToken);
        if (platform is null)
        {
            return null;
        }
        return new PlatformDto(platform.Id, platform.PlatformName, platform.CreatedAt);
    }
}