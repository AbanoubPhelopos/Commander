using commander.application.Features.Platforms.DTOs;
using MediatR;

namespace commander.application.Features.Platforms.Queries.GetAll;

public record GetAllPlatformsQuery : IRequest<IEnumerable<PlatformDto>>;
