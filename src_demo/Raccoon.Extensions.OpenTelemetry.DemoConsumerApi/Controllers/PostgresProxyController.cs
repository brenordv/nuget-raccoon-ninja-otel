using Microsoft.AspNetCore.Mvc;
using Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Clients;
using Refit;

namespace Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Controllers;

[ApiController]
[Route("api/demo/postgres")]
public class PostgresProxyController(
    IDemoApiClient demoApiClient,
    ILogger<PostgresProxyController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Proxying request to Demo API /api/demo/postgres");

            using var response = await demoApiClient.GetPostgresVersionAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Demo API returned PostgreSQL version successfully");
                return Ok(response.Content);
            }

            logger.LogWarning("Demo API returned {StatusCode}", response.StatusCode);
            return StatusCode((int)response.StatusCode);
        }
        catch (ApiException ex)
        {
            logger.LogError(ex, "Failed to reach Demo API /api/demo/postgres");
            return StatusCode(502, new { Error = ex.Message });
        }
    }
}