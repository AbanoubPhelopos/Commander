using Commander.Api.Registrations;
using commander.application.Features.Platforms.Commands.Create;
using commander.application.Features.Platforms.Queries.GetAll;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreatePlatformCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetAllPlatformsQuery).Assembly);
});

builder.Services.AddDependencies(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync().ConfigureAwait(false);
