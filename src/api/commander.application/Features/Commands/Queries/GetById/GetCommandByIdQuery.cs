using commander.application.Features.Commands.Dtos;
using MediatR;

namespace commander.application.Features.Commands.Queries.GetById;

public record GetCommandByIdQuery(int Id) : IRequest<CommandsDto?>;