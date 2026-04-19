namespace commander.application.Features.Commands.Dtos;

public record CommandsDto(int Id, string HowTo, string CommandLine, int PlatformId, string PlatformName, DateTime CreatedAt);