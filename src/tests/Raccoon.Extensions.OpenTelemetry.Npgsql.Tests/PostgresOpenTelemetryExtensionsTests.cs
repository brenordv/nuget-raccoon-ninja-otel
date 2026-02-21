using Raccoon.Extensions.OpenTelemetry.Npgsql.Extensions;

namespace Raccoon.Extensions.OpenTelemetry.Npgsql.Tests;

public class PostgresOpenTelemetryExtensionsTests
{
    [Fact]
    public void WithNpgsql_WhenCalled_AddsTracingContributor()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        options.WithNpgsql();

        // Assert
        Assert.Single(options.TracingContributors);
    }

    [Fact]
    public void WithNpgsql_WhenCalled_DoesNotAddMetricsContributor()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        options.WithNpgsql();

        // Assert
        Assert.Empty(options.MetricsContributors);
    }

    [Fact]
    public void WithNpgsql_WhenCalled_ReturnsSameOptionsInstance()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        var result = options.WithNpgsql();

        // Assert
        Assert.Same(options, result);
    }

    [Fact]
    public void WithNpgsql_WhenCalledMultipleTimes_AddsMultipleContributors()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        options.WithNpgsql();
        options.WithNpgsql();

        // Assert
        Assert.Equal(2, options.TracingContributors.Count);
    }
}