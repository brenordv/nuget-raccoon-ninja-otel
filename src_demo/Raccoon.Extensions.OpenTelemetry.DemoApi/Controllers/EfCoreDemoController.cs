using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raccoon.Extensions.OpenTelemetry.DemoApi.Data;
using Raccoon.Extensions.OpenTelemetry.DemoApi.Models;

namespace Raccoon.Extensions.OpenTelemetry.DemoApi.Controllers;

[ApiController]
[Route("api/demo/efcore")]
public class EfCoreDemoController(
    DemoDbContext dbContext,
    ILogger<EfCoreDemoController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Querying watched movies via EF Core");

            var items = await dbContext.WatchedMovie.ToListAsync(cancellationToken);

            logger.LogInformation("EF Core query returned {MovieCount} watched movies", items.Count);
            return items.Count > 0 ? Ok(items) : NoContent();
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "EF Core query for watched movies failed");
            return StatusCode(503, new { Error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] WatchedMovie item, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Adding watched movie {MovieTitle} ({ReleaseYear})", item.Title, item.ReleaseYear);

            dbContext.WatchedMovie.Add(item);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Watched movie {MovieTitle} saved successfully", item.Title);
            return CreatedAtAction(nameof(GetAsync), new { id = item.LetterboxdUri }, item);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to save watched movie {MovieTitle}", item.Title);
            return StatusCode(503, new { Error = ex.Message });
        }
    }
}