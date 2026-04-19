using commander.application.Features.Platforms.DTOs;
using MediatR;

namespace commander.application.Features.Platforms.Commands.Create;

public record CreatePlatformCommand(string PlatformName) : IRequest<PlatformDto>;
