using commander.application.Features.Commands.Dtos;
using commander.domain.Common;
using commander.domain.Entities;
using commander.domain.Interfaces;
using Mapster;
using MediatR;

namespace commander.application.Features.Commands.Queries.GetAll;

public class GetAllCommandsQueryHandler(ICommandRepository commandRepository) : IRequestHandler<GetAllCommandsQuery, PaginatedList<CommandsDto>>
{
    private readonly ICommandRepository _commandRepository = commandRepository;

    public async Task<PaginatedList<CommandsDto>> Handle(GetAllCommandsQuery request, CancellationToken cancellationToken)
    {
        PaginatedList<Command> commands = await _commandRepository.GetAllCommandsAsync(request.PaginationParams, request.Search, request.SortBy, request.Descending, cancellationToken).ConfigureAwait(false);
        return new PaginatedList<CommandsDto>(commands.Items.Adapt<List<CommandsDto>>(), commands.PageIndex, commands.PageSize, commands.TotalCount);
    }
}