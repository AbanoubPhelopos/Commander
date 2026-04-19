using commander.application.Features.Commands.Dtos;
using MediatR;

namespace commander.application.Features.Commands.Commands.Create;

public record CreateCommandCommand(string HowTo, string CommandLine, int PlatformId) : IRequest<CommandsDto>;