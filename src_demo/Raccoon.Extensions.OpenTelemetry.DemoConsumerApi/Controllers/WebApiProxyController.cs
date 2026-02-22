using Microsoft.AspNetCore.Mvc;
using Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Clients;
using Refit;

namespace Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Controllers;

[ApiController]
[Route("api/demo/webapi")]
public class WebApiProxyController(
    IDemoApiClient demoApiClient,
    ILogger<WebApiProxyController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Proxying request to Demo API /api/demo/webapi");

            using var response = await demoApiClient.GetServerInfoAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Demo API returned server info for {MachineName}", response.Content!.MachineName);
                return Ok(response.Content);
            }

            logger.LogWarning("Demo API returned {StatusCode}", response.StatusCode);
            return StatusCode((int)response.StatusCode);
        }
        catch (ApiException ex)
        {
            logger.LogError(ex, "Failed to reach Demo API /api/demo/webapi");
            return StatusCode(502, new { Error = ex.Message });
        }
    }
}