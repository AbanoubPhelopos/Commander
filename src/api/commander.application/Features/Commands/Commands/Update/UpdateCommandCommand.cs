using commander.application.Features.Commands.Dtos;
using MediatR;

namespace commander.application.Features.Commands.Commands.Update;

public record UpdateCommandCommand(int Id, string HowTo, string CommandLine, int PlatformId) : IRequest<CommandsDto?>;