using Microsoft.AspNetCore.Mvc;

namespace Raccoon.Extensions.OpenTelemetry.DemoApi.Controllers;

[ApiController]
[Route("api/demo/http")]
public class HttpClientDemoController(
    IHttpClientFactory httpClientFactory,
    ILogger<HttpClientDemoController> logger) : ControllerBase
{
    private const string TargetUrl = "https://httpbin.org/get";

    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Sending outbound HTTP request to {TargetUrl}", TargetUrl);

            var client = httpClientFactory.CreateClient("demo");
            var response = await client.GetStringAsync(TargetUrl, cancellationToken);
            return Ok(new { Response = response });
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Outbound HTTP request to {TargetUrl} failed", TargetUrl);
            return StatusCode(502, new { Error = ex.Message });
        }
    }
}