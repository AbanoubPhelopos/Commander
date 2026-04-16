using commander.domain.Entities;
using commander.infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Commander.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Platform>>> GetPlatforms(CancellationToken cancellationToken)
    {
        List<Platform> platforms = await _context.Platforms.ToListAsync(cancellationToken);
        return Ok(platforms);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Platform>> GetPlatformById(int id, CancellationToken cancellationToken)
    {
        Platform? platform = await _context.Platforms.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (platform is null)
        {
            return NotFound($"Platform with ID {id} not found");
        }

        return Ok(platform);
    }

    [HttpPost]
    public async Task<ActionResult<Platform>> CreatePlatform([FromBody] Platform platform, CancellationToken cancellationToken)
    {
        if (platform is null)
        {
            return BadRequest("Platform can not be null");
        }

        _context.Platforms.Add(platform);
        await _context.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetPlatformById), new { id = platform.Id }, platform);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Platform>> UpdatePlatform(int id, [FromBody] Platform platform, CancellationToken cancellationToken)
    {
        Platform? platformFromContext = await _context.Platforms.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (platformFromContext is null)
        {
            return NotFound($"Platform with ID {id} not found");
        }

        if (platform is null)
        {
            return BadRequest("Platform can not be null");
        }

        platformFromContext.PlatformName = platform.PlatformName;
        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Platform>> DeletePlatform(int id, CancellationToken cancellationToken)
    {
        Platform? platformFromContext = await _context.Platforms.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (platformFromContext is null)
        {
            return NotFound($"Platform with ID {id} not found");
        }

        _context.Platforms.Remove(platformFromContext);
        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}