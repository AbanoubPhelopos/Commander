using commander.application.Features.Commands.Dtos;
using commander.domain.Common;
using MediatR;

namespace commander.application.Features.Commands.Queries.GetAll;

public record GetAllCommandsQuery(PaginationParams PaginationParams, string? Search = null, string? SortBy = null, bool Descending = false) : IRequest<PaginatedList<CommandsDto>>;