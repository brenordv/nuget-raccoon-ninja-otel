using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Raccoon.Extensions.OpenTelemetry.SqlClient.Extensions;

/// <summary>
/// Extension methods for adding SQL Server tracing and metrics to the Raccoon OpenTelemetry setup.
/// </summary>
public static class SqlClientOpenTelemetryExtensions
{
    /// <summary>
    /// Registers SQL Server tracing and metrics instrumentation.
    /// </summary>
    /// <param name="options">The <see cref="OpenTelemetryOptions"/> to configure.</param>
    /// <returns>The same <paramref name="options"/> instance for chaining.</returns>
    public static OpenTelemetryOptions WithSqlClient(this OpenTelemetryOptions options)
    {
        options.TracingContributors.Add(tracing => tracing.AddSqlClientInstrumentation());
        options.MetricsContributors.Add(metrics => metrics.AddSqlClientInstrumentation());
        return options;
    }
}