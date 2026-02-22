using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace Raccoon.Extensions.OpenTelemetry.DemoApi.Controllers;

[ApiController]
[Route("api/demo/cosmos")]
public class CosmosDemoController(
    CosmosClient cosmosClient,
    IConfiguration configuration,
    ILogger<CosmosDemoController> logger) : ControllerBase
{
    /// <summary>
    /// Retrieves properties of a Cosmos DB container including its ID, partition key path, and status code.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An IActionResult containing the container properties if the request is successful,
    /// or an error response if an exception occurs.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var databaseName = configuration["Cosmos:DatabaseName"];
            var containerName = configuration["Cosmos:ContainerName"];

            logger.LogInformation(
                "Reading Cosmos DB container {ContainerName} from database {DatabaseName}",
                containerName, databaseName);

            var container = cosmosClient.GetContainer(databaseName, containerName);
            var properties = await container.ReadContainerAsync(cancellationToken: cancellationToken);

            logger.LogInformation(
                "Cosmos DB container {ContainerId} read successfully with status {StatusCode}",
                properties.Resource.Id, properties.StatusCode);

            return Ok(new
            {
                properties.Resource.Id,
                properties.Resource.PartitionKeyPath,
                properties.StatusCode
            });
        }
        catch (CosmosException ex)
        {
            logger.LogError(ex, "Cosmos DB request failed with status {StatusCode}", ex.StatusCode);
            return StatusCode((int)ex.StatusCode, new { Error = ex.Message });
        }
    }
}