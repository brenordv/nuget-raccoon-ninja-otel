using Microsoft.Extensions.Configuration;
using Raccoon.Extensions.OpenTelemetry.Extensions;
using Raccoon.Extensions.OpenTelemetry.Tests.Common;
using Raccoon.Extensions.OpenTelemetry.Tests.Common.Generators;

namespace Raccoon.Extensions.OpenTelemetry.Tests.Extensions;

public class ConfigurationManagerExtensionsTests
{
    private const string DefaultConfigKey = "OpenTelemetry:OtlpEndpoint";

    [Fact]
    public void GetOpenTelemetryEndpoint_WithNullConfiguration_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            ConfigurationManagerExtensions.GetOpenTelemetryEndpoint(null!));
    }

    [Fact]
    public void GetOpenTelemetryEndpoint_WithNullSectionName_ThrowsArgumentNullException()
    {
        // Arrange
        var configuration = new ConfigurationManager();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            configuration.GetOpenTelemetryEndpoint(sectionName: null!));
    }

    [Theory]
    [MemberData(nameof(TheoryDataGenerator.EmptyOrWhitespaceStrings), MemberType = typeof(TheoryDataGenerator))]
    public void GetOpenTelemetryEndpoint_WithEmptyOrWhitespaceSectionName_ThrowsArgumentException(string sectionName)
    {
        // Arrange
        var configuration = MockDataGenerator.CreateConfigurationManager();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            configuration.GetOpenTelemetryEndpoint(sectionName: sectionName));
    }

    [Fact]
    public void GetOpenTelemetryEndpoint_WithNullKeyName_ThrowsArgumentNullException()
    {
        // Arrange
        var configuration = MockDataGenerator.CreateConfigurationManager();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            configuration.GetOpenTelemetryEndpoint(keyName: null!));
    }

    [Theory]
    [MemberData(nameof(TheoryDataGenerator.EmptyOrWhitespaceStrings), MemberType = typeof(TheoryDataGenerator))]
    public void GetOpenTelemetryEndpoint_WithEmptyOrWhitespaceKeyName_ThrowsArgumentException(string keyName)
    {
        // Arrange
        var configuration = MockDataGenerator.CreateConfigurationManager();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            configuration.GetOpenTelemetryEndpoint(keyName: keyName));
    }

    [Fact]
    public void GetOpenTelemetryEndpoint_WithMissingEndpoint_ThrowsArgumentNullException()
    {
        // Arrange
        var configuration = MockDataGenerator.CreateConfigurationManager();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            configuration.GetOpenTelemetryEndpoint());
    }

    [Theory]
    [MemberData(nameof(TheoryDataGenerator.InvalidUriStrings), MemberType = typeof(TheoryDataGenerator))]
    public void GetOpenTelemetryEndpoint_WithInvalidEndpointUri_ThrowsUriFormatException(string invalidUri)
    {
        // Arrange
        var configuration = MockDataGenerator.CreateConfigurationManager(DefaultConfigKey, invalidUri);

        // Act & Assert
        Assert.Throws<UriFormatException>(() =>
            configuration.GetOpenTelemetryEndpoint());
    }

    [Fact]
    public void GetOpenTelemetryEndpoint_WithValidDefaults_ReturnsExpectedUri()
    {
        // Arrange
        const string defaultEndpointUrl = "http://localhost:4317";
        var configuration = MockDataGenerator.CreateConfigurationManager(DefaultConfigKey, defaultEndpointUrl);

        // Act
        var result = configuration.GetOpenTelemetryEndpoint();

        // Assert
        Assert.Equal(new Uri(defaultEndpointUrl), result);
    }

    [Fact]
    public void GetOpenTelemetryEndpoint_WithCustomSectionAndKey_ReturnsExpectedUri()
    {
        // Arrange
        const string customEndpointUrl = "https://otel.example.com:4317";
        const string customSectionName = "Observability";
        const string customKeyName = "CollectorUrl";
        const string customConfigKey = customSectionName + ":" + customKeyName;
        var configuration = MockDataGenerator.CreateConfigurationManager(customConfigKey, customEndpointUrl);

        // Act
        var result = configuration.GetOpenTelemetryEndpoint(
            sectionName: customSectionName,
            keyName: customKeyName);

        // Assert
        Assert.Equal(new Uri(customEndpointUrl), result);
    }
}