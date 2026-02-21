using Raccoon.Extensions.OpenTelemetry.Models;
using Raccoon.Extensions.OpenTelemetry.SqlClient.Extensions;

namespace Raccoon.Extensions.OpenTelemetry.SqlClient.Tests.Extensions;

public class SqlClientOpenTelemetryExtensionsTests
{
    [Fact]
    public void WithSqlClient_WhenCalled_AddsTracingContributor()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        options.WithSqlClient();

        // Assert
        Assert.Single(options.TracingContributors);
    }

    [Fact]
    public void WithSqlClient_WhenCalled_AddsMetricsContributor()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        options.WithSqlClient();

        // Assert
        Assert.Single(options.MetricsContributors);
    }

    [Fact]
    public void WithSqlClient_WhenCalled_ReturnsSameOptionsInstance()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        var result = options.WithSqlClient();

        // Assert
        Assert.Same(options, result);
    }
}