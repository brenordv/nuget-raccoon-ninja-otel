using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Raccoon.Extensions.OpenTelemetry.Models;

/// <summary>
/// Configuration options for the Raccoon OpenTelemetry setup.
/// </summary>
public sealed record OpenTelemetryOptions
{
    /// <summary>
    /// The OTLP collector endpoint to export telemetry to (e.g., <c>http://localhost:4318</c>).
    /// </summary>
    /// <remarks>
    /// When set, the exporter uses this endpoint with the protocol specified in <see cref="OtlpProtocol"/>.
    /// When <c>null</c> (default), the exporter falls back to the <c>OTEL_EXPORTER_OTLP_ENDPOINT</c> environment variable.
    /// </remarks>
    public Uri OtlpEndpoint { get; set; }

    /// <summary>
    /// The OTLP export protocol to use when <see cref="OtlpEndpoint"/> is set.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="OtlpExportProtocol.HttpProtobuf"/> (port 4318).
    /// Set to <see cref="OtlpExportProtocol.Grpc"/> if your collector listens on gRPC (port 4317).
    /// </remarks>
    public OtlpExportProtocol OtlpProtocol { get; set; } = OtlpExportProtocol.HttpProtobuf;

    /// <summary>
    /// Additional <see cref="System.Diagnostics.ActivitySource"/> names to capture beyond the service name.
    /// </summary>
    public IList<string> AdditionalSources { get; } = [];

    /// <summary>
    /// Extra resource attributes added to the OpenTelemetry resource (e.g., <c>deployment.environment</c>).
    /// </summary>
    public IDictionary<string, object> ResourceAttributes { get; } = new Dictionary<string, object>();

    /// <summary>
    /// Escape hatch for advanced tracing configuration not covered by the default setup.
    /// </summary>
    public Action<TracerProviderBuilder> ConfigureTracing { get; set; }

    /// <summary>
    /// Escape hatch for advanced metrics configuration not covered by the default setup.
    /// </summary>
    public Action<MeterProviderBuilder> ConfigureMetrics { get; set; }

    /// <summary>
    /// Tracing configuration contributors registered by extension packages.
    /// </summary>
    /// <remarks>
    /// Extension packages append to this list via their <c>WithXxx()</c> methods to register additional instrumentation.
    /// </remarks>
    public IList<Action<TracerProviderBuilder>> TracingContributors { get; } = [];

    /// <summary>
    /// Metrics configuration contributors registered by extension packages.
    /// </summary>
    /// <remarks>
    /// Extension packages append to this list via their <c>WithXxx()</c> methods to register additional instrumentation.
    /// </remarks>
    public IList<Action<MeterProviderBuilder>> MetricsContributors { get; } = [];
}