using commander.domain.Interfaces;
using commander.infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Commander.Api.Registrations;

public static class Dependencies
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}