using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Raccoon.Extensions.OpenTelemetry.DemoApi.Controllers;

[ApiController]
[Route("api/demo/postgres")]
public class PostgresDemoController(
    NpgsqlDataSource dataSource,
    ILogger<PostgresDemoController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Querying PostgreSQL server version via Dapper");

            await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
            var version = await connection.QuerySingleAsync<string>(
                new CommandDefinition("SELECT version()", cancellationToken: cancellationToken));

            logger.LogInformation("PostgreSQL version retrieved: {PostgresVersion}", version);
            return Ok(new { Version = version });
        }
        catch (NpgsqlException ex)
        {
            logger.LogError(ex, "PostgreSQL query failed");
            return StatusCode(503, new { Error = ex.Message });
        }
    }
}