using WeatherService.Application.Interfaces;
using WeatherService.Application.Models;
using WeatherService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace WeatherService.Application.Services;

public sealed class WeatherOrchestratorService(
    IWeatherApiClient apiClient,
    IWeatherRepository repository,
    ILogger<WeatherOrchestratorService> logger)
    : IWeatherOrchestratorService
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

                logger.LogInformation(
                    "Weather response stored successfully");

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
                "Failed to retrieve weather data from external provider");
        }

        try
        {
            var latest =
                await repository.GetLatestAsync(
                    cancellationToken);

            if (latest is not null)
            {
                logger.LogWarning(
                    "Returning cached weather data");

                return new WeatherResult
                {
                    RawJson = latest.RawResponse,
                    IsFromCache = true
                };
            }

            logger.LogWarning(
                "No cached weather data found");

            return new WeatherResult
            {
                RawJson = null,
                IsFromCache = true
            };
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to retrieve cached weather data");

            return new WeatherResult
            {
                RawJson = null,
                IsFromCache = true
            };
        }
    }
}