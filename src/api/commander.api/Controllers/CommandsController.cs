using commander.application.Features.Commands.Commands.Create;
using commander.application.Features.Commands.Commands.Delete;
using commander.application.Features.Commands.Commands.Update;
using commander.application.Features.Commands.Dtos;
using commander.application.Features.Commands.Queries.GetAll;
using commander.application.Features.Commands.Queries.GetById;
using commander.application.Features.Commands.Queries.GetByPlatformId;
using commander.domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Api.Controllers;

#pragma warning disable CA1515
[ApiController]
[Route("api/[controller]")]
public class CommandsController(IMediator mediator, ILogger<CommandsController> logger) : ControllerBase
#pragma warning restore CA1515
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<CommandsController> _logger = logger;

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<CommandsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<CommandsDto>>> GetCommands([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken, string? search = null, string? sortBy = null, bool descending = false)
    {
        _logger.LogInformation("Fetching commands with Page: {PageIndex}, PageSize: {PageSize}, Search: {Search}, SortBy: {SortBy}, Descending: {Descending}",
            paginationParams.PageIndex, paginationParams.PageSize, search, sortBy, descending);

        PaginatedList<CommandsDto> commands = await _mediator.Send(new GetAllCommandsQuery(paginationParams, search, sortBy, descending), cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Fetched {Count} commands out of {TotalCount}", commands.Items.Count, commands.TotalCount);
        return Ok(commands);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CommandsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CommandsDto>> GetCommandById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching command with Id: {CommandId}", id);

        CommandsDto? command = await _mediator.Send(new GetCommandByIdQuery(id), cancellationToken).ConfigureAwait(false);

        if (command is null)
        {
            _logger.LogWarning("Command with Id: {CommandId} not found", id);
            return NotFound(new ProblemDetails { Status = 404, Title = "Command not found", Detail = $"Command with ID {id} not found." });
        }

        return Ok(command);
    }

    [HttpGet("platform/{platformId}")]
    [ProducesResponseType(typeof(PaginatedList<CommandsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaginatedList<CommandsDto>>> GetCommandsByPlatformId(int platformId, [FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken, string? search = null, string? sortBy = null, bool descending = false)
    {
        _logger.LogInformation("Fetching commands for PlatformId: {PlatformId}, Page: {PageIndex}, PageSize: {PageSize}",
            platformId, paginationParams.PageIndex, paginationParams.PageSize);

        PaginatedList<CommandsDto> commands = await _mediator.Send(new GetCommandsByPlatformIdQuery(platformId, paginationParams, search, sortBy, descending), cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Fetched {Count} commands for PlatformId: {PlatformId}", commands.Items.Count, platformId);
        return Ok(commands);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CommandsDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CommandsDto>> CreateCommand([FromBody] CreateCommandCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating command with HowTo: {HowTo}, PlatformId: {PlatformId}", request.HowTo, request.PlatformId);

        CommandsDto created = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Command created successfully with Id: {CommandId}", created.Id);
        return CreatedAtAction(nameof(GetCommandById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CommandsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateCommand(int id, [FromBody] UpdateCommandCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Updating command with Id: {CommandId}", id);

        UpdateCommandCommand updateCommand = new(id, request.HowTo, request.CommandLine, request.PlatformId);
        CommandsDto? updated = await _mediator.Send(updateCommand, cancellationToken).ConfigureAwait(false);

        if (updated is null)
        {
            _logger.LogWarning("Command with Id: {CommandId} not found for update", id);
            return NotFound(new ProblemDetails { Status = 404, Title = "Command not found", Detail = $"Command with ID {id} not found." });
        }

        _logger.LogInformation("Command with Id: {CommandId} updated successfully", id);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteCommand(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting command with Id: {CommandId}", id);

        bool deleted = await _mediator.Send(new DeleteCommandCommand(id), cancellationToken).ConfigureAwait(false);

        if (!deleted)
        {
            _logger.LogWarning("Command with Id: {CommandId} not found for deletion", id);
            return NotFound(new ProblemDetails { Status = 404, Title = "Command not found", Detail = $"Command with ID {id} not found." });
        }

        _logger.LogInformation("Command with Id: {CommandId} deleted successfully", id);
        return NoContent();
    }
}
