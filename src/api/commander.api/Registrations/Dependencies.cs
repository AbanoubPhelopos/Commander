using commander.application.Features.Commands.Mappings;
using commander.application.Features.Platforms.Commands.Create;
using commander.application.Features.Platforms.Commands.Update;
using commander.application.Features.Platforms.Mappings;
using commander.domain.Interfaces;
using commander.infrastructure.Persistence;
using commander.infrastructure.Persistence.Repositories;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Commander.Api.Registrations;

internal static class Dependencies
{
    internal static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            + ";Username=" + configuration["DbUserId"]
            + ";Password=" + configuration["DbPassword"];

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IPlatformRepository, PlatformRepository>();
        services.AddScoped<ICommandRepository, CommandRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddValidatorsFromAssemblyContaining<CreatePlatformCommandValidator>();
        TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);

        CommandMappingProfile commandProfile = new();
        commandProfile.Register(TypeAdapterConfig.GlobalSettings);

        PlatformMappingProfile platformProfile = new();
        platformProfile.Register(TypeAdapterConfig.GlobalSettings);

        return services;
    }
}
