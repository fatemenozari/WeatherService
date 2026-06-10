using Microsoft.Extensions.Logging;
using WeatherService.Application.Interfaces;
using WeatherService.Application.Models;
using WeatherService.Domain.Entities;

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
               try
               {
                   await repository.AddAsync(
                       new WeatherRecord(response),
                       cancellationToken);
           
                   logger.LogInformation("Weather response stored successfully");
               }
               catch (Exception ex)
               {
                   logger.LogError(ex,"Failed to store weather response");
               }
           
               return new WeatherResult
               {
                   RawJson = response,
               };
           }

            logger.LogWarning(
                "{Service}.{Method}: Empty response received from weather provider",
                nameof(WeatherOrchestratorService),
                nameof(GetWeatherAsync));
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "{Service}.{Method}: Failed to retrieve weather data from external provider",
                nameof(WeatherOrchestratorService),
                nameof(GetWeatherAsync));
        }

        return await GetCachedWeatherAsync(
            cancellationToken);
    }

    private async Task<WeatherResult> GetCachedWeatherAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            var latest =
                await repository.GetLatestAsync(
                    cancellationToken);

            if (latest is not null)
            {
                logger.LogWarning(
                    "{Service}.{Method}: Returning cached weather data",
                    nameof(WeatherOrchestratorService),
                    nameof(GetCachedWeatherAsync));

                return new WeatherResult
                {
                    RawJson = latest.RawResponse,
                };
            }

            logger.LogWarning(
                "{Service}.{Method}: No cached weather data found",
                nameof(WeatherOrchestratorService),
                nameof(GetCachedWeatherAsync));

            return new WeatherResult
            {
                RawJson = null,
            };
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "{Service}.{Method}: Failed to retrieve cached weather data",
                nameof(WeatherOrchestratorService),
                nameof(GetCachedWeatherAsync));

            return new WeatherResult
            {
                RawJson = null,
            };
        }
    }
}