using WeatherService.Application.Interfaces;

namespace WeatherService.Infrastructure.ExternalServices;

public sealed class OpenMeteoClient(HttpClient httpClient) : IWeatherApiClient
{
    public async Task<string?> GetWeatherAsync(
        CancellationToken cancellationToken)
    {
        var response =
            await httpClient.GetAsync(
                "",
                cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadAsStringAsync(cancellationToken);
    }
}