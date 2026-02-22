using Microsoft.AspNetCore.Mvc;
using Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Clients;
using Refit;

namespace Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Controllers;

[ApiController]
[Route("api/demo/cosmos")]
public class CosmosProxyController(
    IDemoApiClient demoApiClient,
    ILogger<CosmosProxyController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Proxying request to Demo API /api/demo/cosmos");

            using var response = await demoApiClient.GetCosmosContainerAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Demo API returned Cosmos container info successfully");
                return Ok(response.Content);
            }

            logger.LogWarning("Demo API returned {StatusCode}", response.StatusCode);
            return StatusCode((int)response.StatusCode);
        }
        catch (ApiException ex)
        {
            logger.LogError(ex, "Failed to reach Demo API /api/demo/cosmos");
            return StatusCode(502, new { Error = ex.Message });
        }
    }
}