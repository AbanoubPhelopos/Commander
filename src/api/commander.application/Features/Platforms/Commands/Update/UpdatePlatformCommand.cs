using commander.application.Features.Platforms.DTOs;
using MediatR;

namespace commander.application.Features.Platforms.Commands.Update;

public record UpdatePlatformCommand(int Id, string PlatformName, DateTime CreatedAt) : IRequest<PlatformDto?>;
