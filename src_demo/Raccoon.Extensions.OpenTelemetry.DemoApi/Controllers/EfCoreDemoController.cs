using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raccoon.Extensions.OpenTelemetry.DemoApi.Data;
using Raccoon.Extensions.OpenTelemetry.DemoApi.Models;

namespace Raccoon.Extensions.OpenTelemetry.DemoApi.Controllers;

[ApiController]
[Route("api/demo/efcore")]
public class EfCoreDemoController(DemoDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var items = await dbContext.WatchedMovie.ToListAsync(cancellationToken);
            return items.Count > 0 ? Ok(items) : NoContent();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(503, new { Error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] WatchedMovie item, CancellationToken cancellationToken)
    {
        try
        {
            dbContext.WatchedMovie.Add(item);
            await dbContext.SaveChangesAsync(cancellationToken);
            return CreatedAtAction(nameof(GetAsync), new { id = item.LetterboxdUri }, item);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(503, new { Error = ex.Message });
        }
    }
}