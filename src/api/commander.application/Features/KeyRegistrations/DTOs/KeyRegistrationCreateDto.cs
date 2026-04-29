namespace commander.application.Features.KeyRegistrations.DTOs;

public record KeyRegistrationCreateDto
{
    public required string Description { get; init; }

    public required string UserId { get; init; }
}
