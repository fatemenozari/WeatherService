using Microsoft.AspNetCore.Mvc;
using WeatherService.Application.Interfaces;

namespace WeatherService.Api.Controllers;

[ApiController]
[Route("api/weather")]
public class WeatherController(
    IWeatherOrchestratorService weatherOrchestrator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        var result =
            await weatherOrchestrator.GetWeatherAsync(
                cancellationToken);

        return Content(
            result.RawJson ?? "null",
            "application/json");
    }
}