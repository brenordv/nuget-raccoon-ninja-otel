using Raccoon.Extensions.OpenTelemetry.EntityFrameworkCore.Extensions;

namespace Raccoon.Extensions.OpenTelemetry.EntityFrameworkCore.Tests;

public class EfCoreOpenTelemetryExtensionsTests
{
    [Fact]
    public void WithEntityFrameworkCore_WhenCalled_AddsTracingContributor()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        options.WithEntityFrameworkCore();

        // Assert
        Assert.Single(options.TracingContributors);
    }

    [Fact]
    public void WithEntityFrameworkCore_WhenCalled_DoesNotAddMetricsContributor()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        options.WithEntityFrameworkCore();

        // Assert
        Assert.Empty(options.MetricsContributors);
    }

    [Fact]
    public void WithEntityFrameworkCore_WhenCalled_ReturnsSameOptionsInstance()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        var result = options.WithEntityFrameworkCore();

        // Assert
        Assert.Same(options, result);
    }
}