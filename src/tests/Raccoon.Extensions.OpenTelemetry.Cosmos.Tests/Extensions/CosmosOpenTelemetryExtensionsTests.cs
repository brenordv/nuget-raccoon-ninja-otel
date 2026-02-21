using Raccoon.Extensions.OpenTelemetry.Cosmos.Extensions;
using Raccoon.Extensions.OpenTelemetry.Models;

namespace Raccoon.Extensions.OpenTelemetry.Cosmos.Tests;

public class CosmosOpenTelemetryExtensionsTests
{
    [Fact]
    public void WithCosmos_WhenCalled_AddsTracingContributor()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        options.WithCosmos();

        // Assert
        Assert.Single(options.TracingContributors);
    }

    [Fact]
    public void WithCosmos_WhenCalled_DoesNotAddMetricsContributor()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        options.WithCosmos();

        // Assert
        Assert.Empty(options.MetricsContributors);
    }

    [Fact]
    public void WithCosmos_WhenCalled_ReturnsSameOptionsInstance()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        var result = options.WithCosmos();

        // Assert
        Assert.Same(options, result);
    }
}