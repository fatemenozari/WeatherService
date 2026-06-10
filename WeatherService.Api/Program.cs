using WeatherService.Application.DependencyInjection;
using WeatherService.Infrastructure.DependencyInjection;
using Serilog;
using Microsoft.EntityFrameworkCore;
using WeatherService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(
    builder.Configuration);

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console();
});

builder.Services
    .AddHealthChecks()
    .AddSqlServer(
        builder.Configuration
            .GetConnectionString("DefaultConnection")!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapHealthChecks("/health");

using var scope = app.Services.CreateScope();

try
{
    var dbContext =
        scope.ServiceProvider
            .GetRequiredService<
                WeatherDbContext>();

    dbContext.Database.Migrate();
}
catch (Exception ex)
{
    Log.Error(
        ex,
        "Database migration failed");
}

app.Run();

public partial class Program
{
}