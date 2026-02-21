using OpenTelemetry.Trace;
using Raccoon.Extensions.OpenTelemetry.Models;

namespace Raccoon.Extensions.OpenTelemetry.EntityFrameworkCore.Extensions;

/// <summary>
/// Extension methods for adding Entity Framework Core tracing to the Raccoon OpenTelemetry setup.
/// </summary>
public static class EntityFrameworkCoreOpenTelemetryExtensions
{
    /// <summary>
    /// Registers Entity Framework Core tracing instrumentation.
    /// </summary>
    /// <param name="options">The <see cref="OpenTelemetryOptions"/> to configure.</param>
    /// <returns>The same <paramref name="options"/> instance for chaining.</returns>
    public static OpenTelemetryOptions WithEntityFrameworkCore(this OpenTelemetryOptions options)
    {
        options.TracingContributors.Add(tracing => tracing.AddEntityFrameworkCoreInstrumentation());
        return options;
    }
}