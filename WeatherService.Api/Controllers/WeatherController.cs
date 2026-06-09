using Microsoft.AspNetCore.Mvc;
using WeatherService.Application.Services;

namespace WeatherService.Api.Controllers;

[ApiController]
[Route("api/weather")]
public class WeatherController(WeatherOrchestratorService weatherOrchestrator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        var result =
            await weatherOrchestrator.GetWeatherAsync(
                cancellationToken);

        if (result.RawJson is null)
            return Ok(null);

        return Content(
            result.RawJson,
            "application/json");
    }
}