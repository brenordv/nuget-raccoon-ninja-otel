using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Raccoon.Extensions.OpenTelemetry.DemoApi.Controllers;

[ApiController]
[Route("api/demo/postgres")]
public class PostgresDemoController(NpgsqlDataSource dataSource) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
            var version = await connection.QuerySingleAsync<string>(
                new CommandDefinition("SELECT version()", cancellationToken: cancellationToken));
            return Ok(new { Version = version });
        }
        catch (NpgsqlException ex)
        {
            return StatusCode(503, new { Error = ex.Message });
        }
    }
}