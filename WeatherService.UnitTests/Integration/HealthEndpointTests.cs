using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WeatherService.UnitTests.Integration;

public class HealthEndpointTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Health_Should_Return_Success()
    {
        var response =
            await _client.GetAsync("/health");

        response.IsSuccessStatusCode
            .Should()
            .BeTrue();
    }
}