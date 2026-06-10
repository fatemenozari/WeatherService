namespace WeatherService.Domain.Entities;

public sealed class WeatherRecord
{
    public long Id { get; private set; }

    public string RawResponse { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    private WeatherRecord()
    {
    }

    public WeatherRecord(string rawResponse)
    {
        RawResponse = rawResponse;
        CreatedAtUtc = DateTime.UtcNow;
    }
}