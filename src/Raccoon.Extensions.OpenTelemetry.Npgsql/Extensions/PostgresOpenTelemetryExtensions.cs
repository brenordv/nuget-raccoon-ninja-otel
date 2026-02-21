using Npgsql;
using Raccoon.Extensions.OpenTelemetry.Models;

namespace Raccoon.Extensions.OpenTelemetry.Npgsql.Extensions;

/// <summary>
/// Extension methods for adding Npgsql (PostgreSQL) tracing to the Raccoon OpenTelemetry setup.
/// </summary>
public static class PostgresOpenTelemetryExtensions
{
    /// <summary>
    /// Registers Npgsql (PostgreSQL) tracing instrumentation.
    /// </summary>
    /// <param name="options">The <see cref="OpenTelemetryOptions"/> to configure.</param>
    /// <returns>The same <paramref name="options"/> instance for chaining.</returns>
    public static OpenTelemetryOptions WithNpgsql(this OpenTelemetryOptions options)
    {
        options.TracingContributors.Add(tracing => tracing.AddNpgsql());
        return options;
    }
}