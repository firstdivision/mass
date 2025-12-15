using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace mass.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MassDbContext>
{
    public MassDbContext CreateDbContext(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        // Prefer environment variable, fall back to ConnectionStrings:DefaultConnection
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") ??
                               configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "Database connection string is not set. Set the DB_CONNECTION environment variable or add ConnectionStrings:DefaultConnection to appsettings.json.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<MassDbContext>();
        // Configure Npgsql and set the migrations assembly so generated migrations use the `mass.Data` namespace
        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
            npgsqlOptions
            .MigrationsAssembly(typeof(MassDbContext).Assembly.GetName().Name))
            .UseSnakeCaseNamingConvention();

//        ..optionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", "public");

        return new MassDbContext(optionsBuilder.Options);
    }
}
