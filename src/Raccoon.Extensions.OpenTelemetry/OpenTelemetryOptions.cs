using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Raccoon.Extensions.OpenTelemetry;

/// <summary>
/// Configuration options for the Raccoon OpenTelemetry setup.
/// </summary>
public sealed record OpenTelemetryOptions
{
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