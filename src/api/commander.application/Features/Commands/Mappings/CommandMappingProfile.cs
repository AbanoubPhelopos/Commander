using commander.application.Features.Commands.Dtos;
using commander.domain.Entities;
using Mapster;

namespace commander.application.Features.Commands.Mappings;

public class CommandMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        config.NewConfig<Command, CommandsDto>()
            .Map(dest => dest.PlatformName, src => src.Platform != null ? src.Platform.PlatformName : string.Empty);

        config.NewConfig<CommandsDto, Command>()
            .Map(dest => dest.PlatformId, src => src.PlatformId)
            .Ignore(dest => dest.Platform!);
    }
}