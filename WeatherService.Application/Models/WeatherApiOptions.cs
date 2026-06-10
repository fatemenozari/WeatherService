public sealed class WeatherApiOptions
{
    public const string SectionName = "WeatherApi";

    public string Url { get; set; } = string.Empty;

    public int TimeoutSeconds { get; set; }
}