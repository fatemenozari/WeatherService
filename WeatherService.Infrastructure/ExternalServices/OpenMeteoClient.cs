using Microsoft.Extensions.Logging;
using WeatherService.Application.Interfaces;

namespace WeatherService.Infrastructure.ExternalServices;

public sealed class OpenMeteoClient(
    HttpClient httpClient,
    ILogger<OpenMeteoClient> logger)
    : IWeatherApiClient
{
    public async Task<string?> GetWeatherAsync(
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "{Service}.{Method}: Calling weather provider",
            nameof(OpenMeteoClient),
            nameof(GetWeatherAsync));

        var response =
            await httpClient.GetAsync(
                string.Empty,
                cancellationToken);

        response.EnsureSuccessStatusCode();

        logger.LogInformation(
            "{Service}.{Method}: Weather provider responded successfully",
            nameof(OpenMeteoClient),
            nameof(GetWeatherAsync));

        return await response.Content
            .ReadAsStringAsync(cancellationToken);
    }
}