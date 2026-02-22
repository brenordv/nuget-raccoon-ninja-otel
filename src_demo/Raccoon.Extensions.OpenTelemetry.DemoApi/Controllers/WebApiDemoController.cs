using Microsoft.AspNetCore.Mvc;

namespace Raccoon.Extensions.OpenTelemetry.DemoApi.Controllers;

[ApiController]
[Route("api/demo/webapi")]
public class WebApiDemoController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            MachineName = Environment.MachineName,
            Timestamp = DateTimeOffset.UtcNow,
            OsDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription
        });
    }
}