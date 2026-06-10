using Microsoft.Extensions.DependencyInjection;
using WeatherService.Application.Interfaces;
using WeatherService.Application.Services;

namespace WeatherService.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<
            IWeatherOrchestratorService,
            WeatherOrchestratorService>();

        return services;
    }
}