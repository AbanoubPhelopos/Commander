using commander.application.Features.Commands.Commands.Create;
using commander.application.Features.Commands.Commands.Delete;
using commander.application.Features.Commands.Commands.Update;
using commander.application.Features.Commands.Dtos;
using commander.application.Features.Commands.Queries.GetAll;
using commander.application.Features.Commands.Queries.GetById;
using commander.application.Features.Commands.Queries.GetByPlatformId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Api.Controllers;

#pragma warning disable CA1515
[ApiController]
[Route("api/[controller]")]
public class CommandsController(IMediator mediator) : ControllerBase
#pragma warning restore CA1515
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CommandsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CommandsDto>>> GetCommands(CancellationToken cancellationToken)
    {
        IEnumerable<CommandsDto> commands = await _mediator.Send(new GetAllCommandsQuery(), cancellationToken).ConfigureAwait(false);
        return Ok(commands);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CommandsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CommandsDto>> GetCommandById(int id, CancellationToken cancellationToken)
    {
        CommandsDto? command = await _mediator.Send(new GetCommandByIdQuery(id), cancellationToken).ConfigureAwait(false);

        return command is null
            ? NotFound(new ProblemDetails { Status = 404, Title = "Command not found", Detail = $"Command with ID {id} not found." })
            : Ok(command);
    }

    [HttpGet("platform/{platformId}")]
    [ProducesResponseType(typeof(IEnumerable<CommandsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<CommandsDto>>> GetCommandsByPlatformId(int platformId, CancellationToken cancellationToken)
    {
        IEnumerable<CommandsDto> commands = await _mediator.Send(new GetCommandsByPlatformIdQuery(platformId), cancellationToken).ConfigureAwait(false);
        return Ok(commands);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CommandsDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CommandsDto>> CreateCommand([FromBody] CreateCommandCommand request, CancellationToken cancellationToken)
    {
        CommandsDto created = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetCommandById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CommandsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateCommand(int id, [FromBody] UpdateCommandCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        UpdateCommandCommand updateCommand = new(id, request.HowTo, request.CommandLine, request.PlatformId);
        CommandsDto? updated = await _mediator.Send(updateCommand, cancellationToken).ConfigureAwait(false);

        return updated is null
            ? NotFound(new ProblemDetails { Status = 404, Title = "Command not found", Detail = $"Command with ID {id} not found." })
            : Ok(updated);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteCommand(int id, CancellationToken cancellationToken)
    {
        bool deleted = await _mediator.Send(new DeleteCommandCommand(id), cancellationToken).ConfigureAwait(false);

        return !deleted
            ? NotFound(new ProblemDetails { Status = 404, Title = "Command not found", Detail = $"Command with ID {id} not found." })
            : NoContent();
    }
}