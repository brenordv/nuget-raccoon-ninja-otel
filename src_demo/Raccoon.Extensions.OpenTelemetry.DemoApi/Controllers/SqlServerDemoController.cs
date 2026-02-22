using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Raccoon.Extensions.OpenTelemetry.DemoApi.Controllers;

[ApiController]
[Route("api/demo/sqlserver")]
public class SqlServerDemoController(IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var connectionString = configuration.GetConnectionString("SqlServer");
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);
            await using var command = new SqlCommand("SELECT @@VERSION", connection);
            var version = await command.ExecuteScalarAsync(cancellationToken);
            return Ok(new { Version = version?.ToString() });
        }
        catch (SqlException ex)
        {
            return StatusCode(503, new { Error = ex.Message });
        }
    }
}