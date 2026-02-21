using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Raccoon.Extensions.OpenTelemetry.Extensions;

/// <summary>
/// Extension methods for configuring OpenTelemetry on any <see cref="IHostApplicationBuilder"/>.
/// </summary>
public static class OpenTelemetryBuilderExtensions
{
    /// <summary>
    /// Adds OpenTelemetry tracing, metrics, and logging to any .NET application with sensible defaults.
    /// </summary>
    /// <typeparam name="TBuilder">The host application builder type.</typeparam>
    /// <param name="builder">The host application builder instance.</param>
    /// <param name="serviceName">
    /// The logical service name used as the OpenTelemetry resource <c>service.name</c> and as the default <see cref="System.Diagnostics.ActivitySource"/> name.
    /// </param>
    /// <param name="configure">
    /// Optional callback to customize <see cref="OpenTelemetryOptions"/> and register extension package contributions (e.g., <c>o.WithNpgsql()</c>).
    /// </param>
    /// <returns>The same <paramref name="builder"/> instance for chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="serviceName"/> is null or whitespace.</exception>
    /// <remarks>
    /// <para>
    /// This method configures the OTLP exporter via <c>UseOtlpExporter()</c>, which reads the
    /// <c>OTEL_EXPORTER_OTLP_ENDPOINT</c> environment variable. It cannot coexist with per-signal
    /// <c>AddOtlpExporter()</c> calls — this package owns the full OTel pipeline.
    /// </para>
    /// <para>
    /// The core package is host-agnostic. For ASP.NET Core instrumentation, add the
    /// <c>Raccoon.Extensions.OpenTelemetry.WebApi</c> package and call <c>o.WithWebApi()</c>.
    /// </para>
    /// </remarks>
    public static TBuilder AddOpenTelemetry<TBuilder>(
        this TBuilder builder,
        string serviceName,
        Action<OpenTelemetryOptions> configure = null)
        where TBuilder : IHostApplicationBuilder
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serviceName);

        var options = new OpenTelemetryOptions();
        configure?.Invoke(options);

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(serviceName);

                if (options.ResourceAttributes.Count > 0)
                {
                    resource.AddAttributes(options.ResourceAttributes);
                }
            })
            .WithTracing(tracing =>
            {
                tracing.AddSource(serviceName);

                foreach (var source in options.AdditionalSources)
                {
                    tracing.AddSource(source);
                }

                tracing.AddHttpClientInstrumentation();

                foreach (var contributor in options.TracingContributors)
                {
                    contributor(tracing);
                }

                options.ConfigureTracing?.Invoke(tracing);
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();

                foreach (var contributor in options.MetricsContributors)
                {
                    contributor(metrics);
                }

                options.ConfigureMetrics?.Invoke(metrics);
            })
            .UseOtlpExporter();

        return builder;
    }
}