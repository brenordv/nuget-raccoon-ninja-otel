using Microsoft.AspNetCore.Mvc;
using Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Clients;
using Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Models;
using Refit;

namespace Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Controllers;

[ApiController]
[Route("api/demo/efcore")]
public class EfCoreProxyController(
    IDemoApiClient demoApiClient,
    ILogger<EfCoreProxyController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Proxying request to Demo API /api/demo/efcore");

            using var response = await demoApiClient.GetWatchedMoviesAsync(cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent || (response.IsSuccessStatusCode && response.Content is null or []))
            {
                logger.LogInformation("Demo API returned no watched movies");
                return NoContent();
            }

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Demo API returned {Count} watched movies", response.Content!.Count);
                return Ok(response.Content);
            }

            logger.LogWarning("Demo API returned {StatusCode}", response.StatusCode);
            return StatusCode((int)response.StatusCode);
        }
        catch (ApiException ex)
        {
            logger.LogError(ex, "Failed to reach Demo API /api/demo/efcore");
            return StatusCode(502, new { Error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(
        [FromBody] WatchedMovieResponse movie,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Proxying POST request to Demo API /api/demo/efcore for movie {Title}", movie.Title);

            using var response = await demoApiClient.PostWatchedMovieAsync(movie, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Demo API saved watched movie {Title} successfully", movie.Title);
                return StatusCode((int)response.StatusCode, response.Content);
            }

            logger.LogWarning("Demo API returned {StatusCode}", response.StatusCode);
            return StatusCode((int)response.StatusCode);
        }
        catch (ApiException ex)
        {
            logger.LogError(ex, "Failed to reach Demo API /api/demo/efcore");
            return StatusCode(502, new { Error = ex.Message });
        }
    }
}