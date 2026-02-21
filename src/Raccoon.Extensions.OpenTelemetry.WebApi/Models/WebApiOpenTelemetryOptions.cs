namespace Raccoon.Extensions.OpenTelemetry.WebApi.Models;

/// <summary>
/// Configuration options for ASP.NET Core OpenTelemetry instrumentation.
/// </summary>
public sealed record WebApiOpenTelemetryOptions
{
    /// <summary>
    /// Request paths excluded from tracing. Requests matching these path prefixes will not generate trace spans.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>["/health", "/alive"]</c>. Uses <c>PathString.StartsWithSegments</c> for matching.
    /// </remarks>
    public IList<string> ExcludedPaths { get; init; } = ["/health", "/alive"];
}