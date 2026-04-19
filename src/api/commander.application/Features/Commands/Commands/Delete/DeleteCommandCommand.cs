using MediatR;

namespace commander.application.Features.Commands.Commands.Delete;

public record DeleteCommandCommand(int Id) : IRequest<bool>;