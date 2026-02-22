using Microsoft.Extensions.Configuration;

namespace Raccoon.Extensions.OpenTelemetry.Tests.Common.Generators;

public static partial class MockDataGenerator
{
    /// <summary>
    /// Creates a new instance of the <see cref="ConfigurationManager"/> with a single key-value pair.
    /// </summary>
    /// <param name="key">The configuration key to add to the manager.</param>
    /// <param name="value">The value associated with the configuration key.</param>
    /// <returns>A new instance of <see cref="ConfigurationManager"/> containing the provided key-value pair.</returns>
    public static ConfigurationManager CreateConfigurationManager(string key, string value)
    {
        return CreateConfigurationManager(new Dictionary<string, string> { { key, value } });
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ConfigurationManager"/> with a single key-value pair.
    /// </summary>
    /// <param name="data">Dictionary that will be translated to data inside the Configuration manager.</param>
    /// <returns>A new instance of <see cref="ConfigurationManager"/> containing the specified key-value pair.</returns>
    public static ConfigurationManager CreateConfigurationManager(IDictionary<string, string> data = null)
    {
        var configuration = new ConfigurationManager();
        if (data is null)
        {
            return configuration;
        }

        foreach (var (key, value) in data)
        {
            configuration[key] = value;
        }

        return configuration;
    }
}