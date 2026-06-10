using WeatherService.Application.Models;

namespace WeatherService.Application.Interfaces;

public interface IWeatherOrchestratorService
{
    Task<WeatherResult> GetWeatherAsync(
        CancellationToken cancellationToken);
}