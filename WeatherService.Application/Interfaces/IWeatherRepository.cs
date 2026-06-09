using WeatherService.Domain.Entities;

namespace WeatherService.Application.Interfaces;

public interface IWeatherRepository
{
    Task AddAsync(
        WeatherRecord record,
        CancellationToken cancellationToken);

    Task<WeatherRecord?> GetLatestAsync(
        CancellationToken cancellationToken);
}