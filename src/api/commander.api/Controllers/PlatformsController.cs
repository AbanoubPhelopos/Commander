using commander.application.Features.Platforms.Commands.Create;
using commander.application.Features.Platforms.Commands.Delete;
using commander.application.Features.Platforms.Commands.Update;
using commander.application.Features.Platforms.DTOs;
using commander.application.Features.Platforms.Queries.GetAll;
using commander.application.Features.Platforms.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Api.Controllers;

#pragma warning disable CA1515
[ApiController]
[Route("api/[controller]")]
public sealed class PlatformsController(IMediator mediator) : ControllerBase
#pragma warning restore CA1515
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PlatformDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PlatformDto>>> GetPlatforms(CancellationToken cancellationToken)
    {
        IEnumerable<PlatformDto> platforms = await _mediator.Send(new GetAllPlatformsQuery(), cancellationToken).ConfigureAwait(false);
        return Ok(platforms);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PlatformDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlatformDto>> GetPlatformById(int id, CancellationToken cancellationToken)
    {
        PlatformDto? platform = await _mediator.Send(new GetPlatformByIdQuery(id), cancellationToken).ConfigureAwait(false);

        return platform is null
            ? NotFound(new ProblemDetails { Status = 404, Title = "Platform not found", Detail = $"Platform with ID {id} not found." })
            : Ok(platform);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PlatformDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PlatformDto>> CreatePlatform([FromBody] CreatePlatformCommand command, CancellationToken cancellationToken)
    {
        PlatformDto platform = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetPlatformById), new { id = platform.Id }, platform);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdatePlatform(int id, [FromBody] UpdatePlatformCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        UpdatePlatformCommand updateCommand = new(id, command.PlatformName, command.CreatedAt);
        PlatformDto? platform = await _mediator.Send(updateCommand, cancellationToken).ConfigureAwait(false);

        return platform is null
            ? NotFound(new ProblemDetails { Status = 404, Title = "Platform not found", Detail = $"Platform with ID {id} not found." })
            : NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeletePlatform(int id, CancellationToken cancellationToken)
    {
        bool deleted = await _mediator.Send(new DeletePlatformCommand(id), cancellationToken).ConfigureAwait(false);

        return !deleted
            ? NotFound(new ProblemDetails { Status = 404, Title = "Platform not found", Detail = $"Platform with ID {id} not found." })
            : NoContent();
    }
}
