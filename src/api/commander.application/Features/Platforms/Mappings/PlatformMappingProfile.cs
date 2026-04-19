using commander.application.Features.Platforms.DTOs;
using commander.domain.Entities;
using Mapster;

namespace commander.application.Features.Platforms.Mappings;

public class PlatformMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        config.NewConfig<Platform, PlatformDto>();

        config.NewConfig<PlatformDto, Platform>();
    }
}
