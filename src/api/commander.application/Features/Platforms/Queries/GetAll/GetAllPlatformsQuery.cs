using commander.application.Features.Platforms.DTOs;
using commander.domain.Common;
using MediatR;

namespace commander.application.Features.Platforms.Queries.GetAll;

public record GetAllPlatformsQuery(PaginationParams PaginationParams, string? Search = null, string? SortBy = null, bool Descending = false) : IRequest<PaginatedList<PlatformDto>>;
