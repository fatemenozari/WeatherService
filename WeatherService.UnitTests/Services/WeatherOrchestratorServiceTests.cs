using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WeatherService.Application.Interfaces;
using WeatherService.Application.Services;
using WeatherService.Domain.Entities;

namespace WeatherService.Tests.Services;

public class WeatherOrchestratorServiceTests
{
    [Fact]
    public async Task Should_Return_Weather_When_Api_Succeeds()
    {
        var apiClient =
            new Mock<IWeatherApiClient>();

        var repository =
            new Mock<IWeatherRepository>();

        var logger =
            new Mock<ILogger<WeatherOrchestratorService>>();

        const string response =
            "{\"temperature\":25}";

        apiClient
            .Setup(x => x.GetWeatherAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var service =
            new WeatherOrchestratorService(
                apiClient.Object,
                repository.Object,
                logger.Object);

        var result =
            await service.GetWeatherAsync(
                CancellationToken.None);

        result.RawJson.Should().Be(response);

        result.IsFromCache.Should().BeFalse();

        repository.Verify(
            x => x.AddAsync(
                It.IsAny<WeatherRecord>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task Should_Return_Null_When_Api_And_Database_Fail()
    {
        var apiClient =
            new Mock<IWeatherApiClient>();

        var repository =
            new Mock<IWeatherRepository>();

        var logger =
            new Mock<ILogger<WeatherOrchestratorService>>();

        apiClient
            .Setup(x => x.GetWeatherAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException());

        repository
            .Setup(x => x.GetLatestAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var service =
            new WeatherOrchestratorService(
                apiClient.Object,
                repository.Object,
                logger.Object);

        var result =
            await service.GetWeatherAsync(
                CancellationToken.None);

        result.RawJson
            .Should()
            .BeNull();

        result.IsFromCache
            .Should()
            .BeTrue();
    }
    
    [Fact]
    public async Task Should_Return_Cached_Data_When_Api_Fails()
    {
        var apiClient =
            new Mock<IWeatherApiClient>();

        var repository =
            new Mock<IWeatherRepository>();

        var logger =
            new Mock<ILogger<WeatherOrchestratorService>>();

        apiClient
            .Setup(x => x.GetWeatherAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException());

        repository
            .Setup(x => x.GetLatestAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new WeatherRecord(
                    "{\"temperature\":20}"));

        var service =
            new WeatherOrchestratorService(
                apiClient.Object,
                repository.Object,
                logger.Object);

        var result =
            await service.GetWeatherAsync(
                CancellationToken.None);

        result.RawJson
            .Should()
            .Be("{\"temperature\":20}");

        result.IsFromCache
            .Should()
            .BeTrue();
    }
    
    [Fact]
    public async Task Should_Return_Cached_Data_When_Api_Returns_Empty_Response()
    {
        var apiClient = new Mock<IWeatherApiClient>();
        var repository = new Mock<IWeatherRepository>();
        var logger = new Mock<ILogger<WeatherOrchestratorService>>();

        apiClient
            .Setup(x => x.GetWeatherAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(string.Empty);

        repository
            .Setup(x => x.GetLatestAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new WeatherRecord("{\"temperature\":22}"));

        var service = new WeatherOrchestratorService(
            apiClient.Object,
            repository.Object,
            logger.Object);

        var result =
            await service.GetWeatherAsync(CancellationToken.None);

        result.IsFromCache.Should().BeTrue();
        result.RawJson.Should().Be("{\"temperature\":22}");
    }
}