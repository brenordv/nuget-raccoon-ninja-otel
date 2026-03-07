using Npgsql;
using OpenTelemetry.Trace;
using Raccoon.Extensions.OpenTelemetry.Models;
using Raccoon.Extensions.OpenTelemetry.Npgsql.Processors;

namespace Raccoon.Extensions.OpenTelemetry.Npgsql.Extensions;

/// <summary>
/// Extension methods for adding Npgsql (PostgreSQL) tracing to the Raccoon OpenTelemetry setup.
/// </summary>
public static class PostgresOpenTelemetryExtensions
{
    /// <summary>
    /// Registers Npgsql (PostgreSQL) tracing instrumentation with service graph support.
    /// </summary>
    /// <param name="options">The <see cref="OpenTelemetryOptions"/> to configure.</param>
    /// <returns>The same <paramref name="options"/> instance for chaining.</returns>
    /// <remarks>
    /// In addition to enabling Npgsql tracing, this method registers a span processor that adds
    /// <c>peer.service</c> and <c>db.system</c> attributes to Npgsql spans. This ensures PostgreSQL
    /// appears as a distinct node in observability service graphs (e.g., Grafana Tempo) without
    /// requiring backend configuration changes.
    /// </remarks>
    public static OpenTelemetryOptions WithNpgsql(this OpenTelemetryOptions options)
    {
        options.TracingContributors.Add(tracing =>
        {
            tracing.AddNpgsql();
            tracing.AddProcessor(new NpgsqlPeerServiceProcessor());
        });
        return options;
    }
}