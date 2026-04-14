using commander.infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var connectionString = new NpgsqlConnectionStringBuilder
{
    ConnectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection"),
    Username = builder.Configuration["DbUserId"],
    Password = builder.Configuration["DbPassword"]
};

builder.Services.AddDbContext<AppDbContext>(options =>    
  options.UseNpgsql(connectionString.ConnectionString));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync().ConfigureAwait(false);