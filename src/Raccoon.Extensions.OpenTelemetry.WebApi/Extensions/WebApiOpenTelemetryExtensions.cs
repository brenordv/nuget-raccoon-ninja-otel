using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Raccoon.Extensions.OpenTelemetry.Models;
using Raccoon.Extensions.OpenTelemetry.WebApi.Models;

namespace Raccoon.Extensions.OpenTelemetry.WebApi.Extensions;

/// <summary>
/// Extension methods for adding ASP.NET Core tracing and metrics to the Raccoon OpenTelemetry setup.
/// </summary>
public static class WebApiOpenTelemetryExtensions
{
    /// <summary>
    /// Registers ASP.NET Core tracing and metrics instrumentation.
    /// </summary>
    /// <param name="options">The <see cref="OpenTelemetryOptions"/> to configure.</param>
    /// <param name="configure">Optional callback to customize <see cref="WebApiOpenTelemetryOptions"/>.</param>
    /// <returns>The same <paramref name="options"/> instance for chaining.</returns>
    public static OpenTelemetryOptions WithWebApi(
        this OpenTelemetryOptions options,
        Action<WebApiOpenTelemetryOptions> configure = null)
    {
        var webApiOptions = new WebApiOpenTelemetryOptions();
        configure?.Invoke(webApiOptions);

        options.TracingContributors.Add(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation(aspnet =>
            {
                aspnet.Filter = ctx => !webApiOptions.ExcludedPaths
                    .Any(p => ctx.Request.Path.StartsWithSegments(p));
            });
        });

        options.MetricsContributors.Add(metrics => metrics.AddAspNetCoreInstrumentation());

        return options;
    }
}