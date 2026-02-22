using Microsoft.AspNetCore.Mvc;

namespace Raccoon.Extensions.OpenTelemetry.DemoApi.Controllers;

[ApiController]
[Route("api/demo/http")]
public class HttpClientDemoController(IHttpClientFactory httpClientFactory) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var client = httpClientFactory.CreateClient("demo");
            var response = await client.GetStringAsync("https://httpbin.org/get", cancellationToken);
            return Ok(new { Response = response });
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(502, new { Error = ex.Message });
        }
    }
}