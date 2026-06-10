using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WeatherService.Application.Interfaces;
using WeatherService.Infrastructure.ExternalServices;
using WeatherService.Infrastructure.Persistence;
using WeatherService.Infrastructure.Repositories;

namespace WeatherService.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<WeatherDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });

        services.Configure<WeatherApiOptions>(
            configuration.GetSection(
                WeatherApiOptions.SectionName));

        services.AddScoped<IWeatherRepository, WeatherRepository>();

        services.AddHttpClient<IWeatherApiClient, OpenMeteoClient>(
                (provider, client) =>
                {
                    var options =
                        provider.GetRequiredService<
                            IOptions<WeatherApiOptions>>();

                    client.BaseAddress =
                        new Uri(options.Value.Url);

                    client.Timeout =
                        TimeSpan.FromSeconds(
                            options.Value.TimeoutSeconds);
                })
            .AddStandardResilienceHandler();

        return services;
    }
}