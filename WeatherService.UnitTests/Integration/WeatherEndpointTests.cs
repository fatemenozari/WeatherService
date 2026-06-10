using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WeatherService.UnitTests.Integration;

public class WeatherEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Weather_Endpoint_Should_Return_Response()
    {
        var response =
            await _client.GetAsync("/api/weather");

        response.IsSuccessStatusCode
            .Should()
            .BeTrue();
    }
}