using commander.application.Features.Commands.Dtos;
using commander.domain.Common;
using MediatR;

namespace commander.application.Features.Commands.Queries.GetAll;

public record GetAllCommandsQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<CommandsDto>>;