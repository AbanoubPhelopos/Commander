using commander.application.Features.Commands.Dtos;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;

namespace commander.application.Features.Commands.Commands.Create;

public class CreateCommandCommandHandler(IUnitOfWork unitOfWork)
                : IRequestHandler<CreateCommandCommand, CommandsDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CommandsDto> Handle(CreateCommandCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Command command = new()
        {
            HowTo = request.HowTo,
            CommandLine = request.CommandLine,
            PlatformId = request.PlatformId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Repository<Command>().AddAsync(command, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return command.Adapt<CommandsDto>();
    }
}