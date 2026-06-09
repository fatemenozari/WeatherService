namespace WeatherService.Application.Models;

public sealed class WeatherResult
{
    public string? RawJson { get; init; }

    public bool IsFromCache { get; init; }
}