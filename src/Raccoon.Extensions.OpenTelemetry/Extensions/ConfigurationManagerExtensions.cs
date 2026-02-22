using Microsoft.Extensions.Configuration;

namespace Raccoon.Extensions.OpenTelemetry.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IConfigurationManager"/> to handle configuration-related tasks
/// for OpenTelemetry instrumentation and other related functionality.
/// </summary>
public static class ConfigurationManagerExtensions
{
    /// <summary>
    /// Retrieves the OpenTelemetry endpoint URI from the specified configuration
    /// using the provided section and key names.
    /// </summary>
    /// <param name="configuration">The configuration manager to retrieve the endpoint from.</param>
    /// <param name="sectionName">The name of the configuration section containing the OpenTelemetry settings. Defaults to "OpenTelemetry".</param>
    /// <param name="keyName">The name of the key specifying the endpoint URI within the configuration section. Defaults to "OtlpEndpoint".</param>
    /// <returns>The URI of the OpenTelemetry endpoint.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configuration"/> is null.</exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="sectionName"/> or <paramref name="keyName"/> is null, empty, or whitespace,
    /// or when the endpoint URI specified in the configuration is null, empty, or whitespace.
    /// </exception>
    public static Uri GetOpenTelemetryEndpoint(this IConfigurationManager configuration,
        string sectionName = "OpenTelemetry", string keyName = "OtlpEndpoint")
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);
        ArgumentException.ThrowIfNullOrWhiteSpace(keyName);

        var endpoint = configuration.GetSection(sectionName).GetValue<string>(keyName);
        ArgumentException.ThrowIfNullOrWhiteSpace(endpoint);

        return new Uri(endpoint);
    }
}