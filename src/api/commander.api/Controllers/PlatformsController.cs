using commander.application.Features.Platforms.Commands.Create;
using commander.application.Features.Platforms.Commands.Delete;
using commander.application.Features.Platforms.Commands.Update;
using commander.application.Features.Platforms.Queries.GetAll;
using commander.application.Features.Platforms.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetPlatforms(CancellationToken cancellationToken)
    {
        IEnumerable<object> platforms = await _mediator.Send(new GetAllPlatformsQuery(), cancellationToken);
        return Ok(platforms);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetPlatformById(int id, CancellationToken cancellationToken)
    {
        object? platform = await _mediator.Send(new GetPlatformByIdQuery(id), cancellationToken);

        if (platform is null)
        {
            return NotFound($"Platform with ID {id} not found");
        }

        return Ok(platform);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CreatePlatform([FromBody] CreatePlatformCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.PlatformName))
        {
            return BadRequest("Platform name cannot be null or empty");
        }

        object platform = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetPlatformById), new { id = ((dynamic)platform).Id }, platform);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<object>> UpdatePlatform(int id, [FromBody] UpdatePlatformCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.PlatformName))
        {
            return BadRequest("Platform name cannot be null or empty");
        }

        UpdatePlatformCommand updateCommand = new(id, command.PlatformName);
        object? platform = await _mediator.Send(updateCommand, cancellationToken);

        if (platform is null)
        {
            return NotFound($"Platform with ID {id} not found");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePlatform(int id, CancellationToken cancellationToken)
    {
        bool deleted = await _mediator.Send(new DeletePlatformCommand(id), cancellationToken);

        if (!deleted)
        {
            return NotFound($"Platform with ID {id} not found");
        }

        return NoContent();
    }
}