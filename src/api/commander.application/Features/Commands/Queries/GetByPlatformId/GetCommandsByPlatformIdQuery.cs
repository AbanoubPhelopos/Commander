using commander.application.Features.Commands.Dtos;
using commander.domain.Common;
using MediatR;

namespace commander.application.Features.Commands.Queries.GetByPlatformId;

public record GetCommandsByPlatformIdQuery(int PlatformId, PaginationParams PaginationParams) : IRequest<PaginatedList<CommandsDto>>;