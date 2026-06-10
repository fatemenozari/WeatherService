using Microsoft.Extensions.DependencyInjection;
using WeatherService.Application.Services;

namespace WeatherService.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<WeatherOrchestratorService>();

        return services;
    }
}