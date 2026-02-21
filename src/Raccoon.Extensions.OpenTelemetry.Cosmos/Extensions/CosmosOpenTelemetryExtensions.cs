using Raccoon.Extensions.OpenTelemetry.Models;

namespace Raccoon.Extensions.OpenTelemetry.Cosmos.Extensions;

/// <summary>
/// Extension methods for adding Azure Cosmos DB tracing to the Raccoon OpenTelemetry setup.
/// </summary>
public static class CosmosOpenTelemetryExtensions
{
    /// <summary>
    /// Registers Azure Cosmos DB tracing instrumentation via <c>Azure.Cosmos.Operation</c> activity source.
    /// </summary>
    /// <param name="options">The <see cref="OpenTelemetryOptions"/> to configure.</param>
    /// <returns>The same <paramref name="options"/> instance for chaining.</returns>
    /// <remarks>
    /// Consumers must also set <c>CosmosClientOptions.CosmosClientTelemetryOptions.DisableDistributedTracing = false</c>
    /// on their <c>CosmosClient</c> for traces to be emitted.
    ///
    /// Right now, this extension is overkill, but this feature is still in experimental preview from Microsoft, so the
    /// final version may (and probably will) come with changes that will justify having a separate package for it.
    /// </remarks>
    public static OpenTelemetryOptions WithCosmos(this OpenTelemetryOptions options)
    {
        AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);
        options.TracingContributors.Add(tracing => tracing.AddSource("Azure.Cosmos.Operation"));
        return options;
    }
}