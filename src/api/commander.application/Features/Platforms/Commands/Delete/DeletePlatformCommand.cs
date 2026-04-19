using MediatR;

namespace commander.application.Features.Platforms.Commands.Delete;

public record DeletePlatformCommand(int Id) : IRequest<bool>;
