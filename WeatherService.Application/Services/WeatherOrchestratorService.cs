using WeatherService.Application.Interfaces;
using WeatherService.Application.Models;
using WeatherService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace WeatherService.Application.Services;

public sealed class WeatherOrchestratorService(
    IWeatherApiClient apiClient,
    IWeatherRepository repository,
    ILogger<WeatherOrchestratorService> logger)
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
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "{MethodName} failed to retrieve weather from external provider.",
                nameof(GetWeatherAsync));
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
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "{MethodName} failed to retrieve weather from database.",
                nameof(GetWeatherAsync));
            
            return new WeatherResult
            {
                RawJson = null,
                IsFromCache = true
            };
        }
    }
}