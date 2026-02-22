using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Raccoon.Extensions.OpenTelemetry.Models;

namespace Raccoon.Extensions.OpenTelemetry.Tests.Models;

public class OpenTelemetryOptionsTests
{
    [Fact]
    public void Constructor_WhenCreated_SetsEmptyAdditionalSources()
    {
        // Arrange & Act
        var options = new OpenTelemetryOptions();

        // Assert
        Assert.Empty(options.AdditionalSources);
    }

    [Fact]
    public void Constructor_WhenCreated_SetsEmptyResourceAttributes()
    {
        // Arrange & Act
        var options = new OpenTelemetryOptions();

        // Assert
        Assert.Empty(options.ResourceAttributes);
    }

    [Fact]
    public void Constructor_WhenCreated_SetsNullConfigureTracing()
    {
        // Arrange & Act
        var options = new OpenTelemetryOptions();

        // Assert
        Assert.Null(options.ConfigureTracing);
    }

    [Fact]
    public void Constructor_WhenCreated_SetsNullConfigureMetrics()
    {
        // Arrange & Act
        var options = new OpenTelemetryOptions();

        // Assert
        Assert.Null(options.ConfigureMetrics);
    }

    [Fact]
    public void Constructor_WhenCreated_SetsEmptyTracingContributors()
    {
        // Arrange & Act
        var options = new OpenTelemetryOptions();

        // Assert
        Assert.Empty(options.TracingContributors);
    }

    [Fact]
    public void Constructor_WhenCreated_SetsEmptyMetricsContributors()
    {
        // Arrange & Act
        var options = new OpenTelemetryOptions();

        // Assert
        Assert.Empty(options.MetricsContributors);
    }

    [Fact]
    public void TracingContributors_WhenContributorAdded_ContainsContributor()
    {
        // Arrange
        var options = new OpenTelemetryOptions();
        Action<TracerProviderBuilder> contributor = _ => { };

        // Act
        options.TracingContributors.Add(contributor);

        // Assert
        Assert.Single(options.TracingContributors);
        Assert.Same(contributor, options.TracingContributors[0]);
    }

    [Fact]
    public void MetricsContributors_WhenContributorAdded_ContainsContributor()
    {
        // Arrange
        var options = new OpenTelemetryOptions();
        Action<MeterProviderBuilder> contributor = _ => { };

        // Act
        options.MetricsContributors.Add(contributor);

        // Assert
        Assert.Single(options.MetricsContributors);
        Assert.Same(contributor, options.MetricsContributors[0]);
    }

    [Fact]
    public void ResourceAttributes_WhenAttributeAdded_ContainsAttribute()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        options.ResourceAttributes["deployment.environment"] = "production";

        // Assert
        Assert.Single(options.ResourceAttributes);
        Assert.Equal("production", options.ResourceAttributes["deployment.environment"]);
    }

    [Fact]
    public void AdditionalSources_WhenSourceAdded_ContainsSource()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        options.AdditionalSources.Add("MyCustomSource");

        // Assert
        Assert.Single(options.AdditionalSources);
        Assert.Equal("MyCustomSource", options.AdditionalSources[0]);
    }

    [Fact]
    public void Constructor_WhenCreated_SetsNullOtlpEndpoint()
    {
        // Arrange & Act
        var options = new OpenTelemetryOptions();

        // Assert
        Assert.Null(options.OtlpEndpoint);
    }

    [Fact]
    public void OtlpEndpoint_WhenSet_ReturnsConfiguredUri()
    {
        // Arrange
        var endpoint = new Uri("http://localhost:4318");

        // Act
        var options = new OpenTelemetryOptions { OtlpEndpoint = endpoint };

        // Assert
        Assert.Same(endpoint, options.OtlpEndpoint);
    }

    [Fact]
    public void Constructor_WhenCreated_SetsHttpProtobufAsDefaultOtlpProtocol()
    {
        // Arrange & Act
        var options = new OpenTelemetryOptions();

        // Assert
        Assert.Equal(OtlpExportProtocol.HttpProtobuf, options.OtlpProtocol);
    }

    [Fact]
    public void OtlpProtocol_WhenSetToGrpc_ReturnsGrpc()
    {
        // Arrange & Act
        var options = new OpenTelemetryOptions { OtlpProtocol = OtlpExportProtocol.Grpc };

        // Assert
        Assert.Equal(OtlpExportProtocol.Grpc, options.OtlpProtocol);
    }
}