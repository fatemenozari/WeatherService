namespace WeatherService.Application.Interfaces;

public interface IWeatherApiClient
{
    Task<string?> GetWeatherAsync(
        CancellationToken cancellationToken);
}