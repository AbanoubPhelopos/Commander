using commander.application.Features.Platforms.Commands.Create;
using commander.application.Features.Platforms.Commands.Update;
using commander.domain.Interfaces;
using commander.infrastructure.Persistence;
using commander.infrastructure.Persistence.Repositories;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Commander.Api.Registrations;

public static class Dependencies
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IPlatformRepository, PlatformRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddValidatorsFromAssemblyContaining<CreatePlatformCommandValidator>();
        TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);

        return services;
    }
}