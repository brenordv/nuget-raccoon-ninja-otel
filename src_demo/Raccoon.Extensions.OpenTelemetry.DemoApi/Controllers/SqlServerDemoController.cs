using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Raccoon.Extensions.OpenTelemetry.DemoApi.Controllers;

[ApiController]
[Route("api/demo/sqlserver")]
public class SqlServerDemoController(
    IConfiguration configuration,
    ILogger<SqlServerDemoController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Querying SQL Server version via SqlClient");

            var connectionString = configuration.GetConnectionString("SqlServer");
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);
            await using var command = new SqlCommand("SELECT @@VERSION", connection);
            var version = await command.ExecuteScalarAsync(cancellationToken);

            logger.LogInformation("SQL Server version retrieved successfully");
            return Ok(new { Version = version?.ToString() });
        }
        catch (SqlException ex)
        {
            logger.LogError(ex, "SQL Server query failed");
            return StatusCode(503, new { Error = ex.Message });
        }
    }
}