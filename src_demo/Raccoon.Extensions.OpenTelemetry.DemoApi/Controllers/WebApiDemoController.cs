using Microsoft.AspNetCore.Mvc;

namespace Raccoon.Extensions.OpenTelemetry.DemoApi.Controllers;

[ApiController]
[Route("api/demo/webapi")]
public class WebApiDemoController(ILogger<WebApiDemoController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        logger.LogInformation("Returning server info for machine {MachineName}", Environment.MachineName);

        return Ok(new
        {
            MachineName = Environment.MachineName,
            Timestamp = DateTimeOffset.UtcNow,
            OsDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription
        });
    }
}