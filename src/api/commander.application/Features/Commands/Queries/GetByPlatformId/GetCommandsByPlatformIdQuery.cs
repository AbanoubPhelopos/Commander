using commander.application.Features.Commands.Dtos;
using MediatR;

namespace commander.application.Features.Commands.Queries.GetByPlatformId;

public record GetCommandsByPlatformIdQuery(int PlatformId) : IRequest<IEnumerable<CommandsDto>>;