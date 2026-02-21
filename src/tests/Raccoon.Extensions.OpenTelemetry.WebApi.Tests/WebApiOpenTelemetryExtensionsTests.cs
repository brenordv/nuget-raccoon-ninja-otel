using Raccoon.Extensions.OpenTelemetry.WebApi.Extensions;

namespace Raccoon.Extensions.OpenTelemetry.WebApi.Tests;

public class WebApiOpenTelemetryExtensionsTests
{
    [Fact]
    public void WithWebApi_WhenCalled_AddsTracingContributor()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        options.WithWebApi();

        // Assert
        Assert.Single(options.TracingContributors);
    }

    [Fact]
    public void WithWebApi_WhenCalled_AddsMetricsContributor()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        options.WithWebApi();

        // Assert
        Assert.Single(options.MetricsContributors);
    }

    [Fact]
    public void WithWebApi_WhenCalled_ReturnsSameOptionsInstance()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        var result = options.WithWebApi();

        // Assert
        Assert.Same(options, result);
    }

    [Fact]
    public void WithWebApi_WithConfigureCallback_InvokesCallback()
    {
        // Arrange
        var options = new OpenTelemetryOptions();
        var callbackInvoked = false;

        // Act
        options.WithWebApi(webApi =>
        {
            callbackInvoked = true;
        });

        // Assert
        Assert.True(callbackInvoked);
    }

    [Fact]
    public void WithWebApi_WithNullConfigure_DoesNotThrow()
    {
        // Arrange
        var options = new OpenTelemetryOptions();

        // Act
        var exception = Record.Exception(() => options.WithWebApi(configure: null));

        // Assert
        Assert.Null(exception);
    }
}