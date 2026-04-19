using commander.application.Features.Commands.Dtos;
using MediatR;

namespace commander.application.Features.Commands.Queries.GetAll;

public record GetAllCommandsQuery : IRequest<IEnumerable<CommandsDto>>;