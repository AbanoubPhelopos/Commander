using commander.application.Features.Platforms.DTOs;
using commander.domain.Common;
using MediatR;

namespace commander.application.Features.Platforms.Queries.GetAll;

public record GetAllPlatformsQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<PlatformDto>>;
