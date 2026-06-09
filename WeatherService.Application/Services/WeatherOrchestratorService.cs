using WeatherService.Application.Interfaces;
using WeatherService.Application.Models;
using WeatherService.Domain.Entities;

namespace WeatherService.Application.Services;

public sealed class WeatherOrchestratorService(
    IWeatherApiClient apiClient,
    IWeatherRepository repository)
{
    public async Task<WeatherResult> GetWeatherAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            var response =
                await apiClient.GetWeatherAsync(
                    cancellationToken);

            if (!string.IsNullOrWhiteSpace(response))
            {
                await repository.AddAsync(
                    new WeatherRecord(response),
                    cancellationToken);

                return new WeatherResult
                {
                    RawJson = response,
                    IsFromCache = false
                };
            }
        }
        catch
        {
        }

        try
        {
            var latest =
                await repository.GetLatestAsync(
                    cancellationToken);

            return new WeatherResult
            {
                RawJson = latest?.RawResponse,
                IsFromCache = true
            };
        }
        catch
        {
            return new WeatherResult
            {
                RawJson = null,
                IsFromCache = true
            };
        }
    }
}