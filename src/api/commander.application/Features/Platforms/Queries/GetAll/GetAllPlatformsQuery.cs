using commander.application.Features.Platforms.DTOs;
using commander.domain.Entities;
using commander.domain.Interfaces;

namespace commander.application.Features.Platforms.Queries.GetAll;

public record GetAllPlatformsQuery : MediatR.IRequest<IEnumerable<PlatformDto>>;

public class GetAllPlatformsQueryHandler(IUnitOfWork unitOfWork)
    : MediatR.IRequestHandler<GetAllPlatformsQuery, IEnumerable<PlatformDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<PlatformDto>> Handle(GetAllPlatformsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Platform> platforms = await _unitOfWork.Repository<Platform>().GetAllAsync(cancellationToken);
        return platforms.Select(p => new PlatformDto(p.Id, p.PlatformName, p.CreatedAt)).ToList();
    }
}