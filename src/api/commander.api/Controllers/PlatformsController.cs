using commander.application.Features.Commands.Dtos;
using commander.application.Features.Commands.Queries.GetByPlatformId;
using commander.application.Features.Platforms.Commands.Create;
using commander.application.Features.Platforms.Commands.Delete;
using commander.application.Features.Platforms.Commands.Update;
using commander.application.Features.Platforms.DTOs;
using commander.application.Features.Platforms.Queries.GetAll;
using commander.application.Features.Platforms.Queries.GetById;
using commander.domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Api.Controllers;

#pragma warning disable CA1515
[ApiController]
[Route("api/[controller]")]
public sealed class PlatformsController(IMediator mediator, ILogger<PlatformsController> logger) : ControllerBase
#pragma warning restore CA1515
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<PlatformsController> _logger = logger;

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<PlatformDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<PlatformDto>>> GetPlatforms([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken, string? search = null, string? sortBy = null, bool descending = false)
    {
        _logger.LogInformation("Fetching platforms with Page: {PageIndex}, PageSize: {PageSize}, Search: {Search}, SortBy: {SortBy}, Descending: {Descending}",
            paginationParams.PageIndex, paginationParams.PageSize, search, sortBy, descending);

        PaginatedList<PlatformDto> platforms = await _mediator.Send(new GetAllPlatformsQuery(paginationParams, search, sortBy, descending), cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Fetched {Count} platforms out of {TotalCount}", platforms.Items.Count, platforms.TotalCount);
        return Ok(platforms);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PlatformDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlatformDto>> GetPlatformById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching platform with Id: {PlatformId}", id);

        PlatformDto? platform = await _mediator.Send(new GetPlatformByIdQuery(id), cancellationToken).ConfigureAwait(false);

        if (platform is null)
        {
            _logger.LogWarning("Platform with Id: {PlatformId} not found", id);
            return NotFound(new ProblemDetails { Status = 404, Title = "Platform not found", Detail = $"Platform with ID {id} not found." });
        }

        return Ok(platform);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PlatformDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PlatformDto>> CreatePlatform([FromBody] CreatePlatformCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating platform with name: {PlatformName}", command.PlatformName);

        PlatformDto platform = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Platform created successfully with Id: {PlatformId}", platform.Id);
        return CreatedAtAction(nameof(GetPlatformById), new { id = platform.Id }, platform);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdatePlatform(int id, [FromBody] UpdatePlatformCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        _logger.LogInformation("Updating platform with Id: {PlatformId}", id);

        UpdatePlatformCommand updateCommand = new(id, command.PlatformName, command.CreatedAt);
        PlatformDto? platform = await _mediator.Send(updateCommand, cancellationToken).ConfigureAwait(false);

        if (platform is null)
        {
            _logger.LogWarning("Platform with Id: {PlatformId} not found for update", id);
            return NotFound(new ProblemDetails { Status = 404, Title = "Platform not found", Detail = $"Platform with ID {id} not found." });
        }

        _logger.LogInformation("Platform with Id: {PlatformId} updated successfully", id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeletePlatform(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting platform with Id: {PlatformId}", id);

        bool deleted = await _mediator.Send(new DeletePlatformCommand(id), cancellationToken).ConfigureAwait(false);

        if (!deleted)
        {
            _logger.LogWarning("Platform with Id: {PlatformId} not found for deletion", id);
            return NotFound(new ProblemDetails { Status = 404, Title = "Platform not found", Detail = $"Platform with ID {id} not found." });
        }

        _logger.LogInformation("Platform with Id: {PlatformId} deleted successfully", id);
        return NoContent();
    }

    [HttpGet("{id}/commands")]
    [ProducesResponseType(typeof(PaginatedList<CommandsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaginatedList<CommandsDto>>> GetCommandsForPlatform(int id, [FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken, string? search = null, string? sortBy = null, bool descending = false)
    {
        _logger.LogInformation("Fetching commands for PlatformId: {PlatformId}, Page: {PageIndex}, PageSize: {PageSize}",
            id, paginationParams.PageIndex, paginationParams.PageSize);

        PaginatedList<CommandsDto> commands = await _mediator.Send(new GetCommandsByPlatformIdQuery(id, paginationParams, search, sortBy, descending), cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Fetched {Count} commands for PlatformId: {PlatformId}", commands.Items.Count, id);
        return Ok(commands);
    }

}
