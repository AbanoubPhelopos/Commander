using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace commander.infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddUserSecrets(Assembly.Load("commander.api"), optional: true)
            .Build();

        string connectionStringBase = configuration.GetConnectionString("PostgreSqlConnection") 
            ?? "Host=localhost;Port=5432;Database=commandapi;Pooling=true;";
        
        string username = configuration["DbUserId"] ?? "postgres";
        string password = configuration["DbPassword"] ?? throw new InvalidOperationException("DbPassword user secret is not set");

        var npgsqlConnectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionStringBase)
        {
            Username = username,
            Password = password
        };

        string connectionString = npgsqlConnectionStringBuilder.ConnectionString;

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}
