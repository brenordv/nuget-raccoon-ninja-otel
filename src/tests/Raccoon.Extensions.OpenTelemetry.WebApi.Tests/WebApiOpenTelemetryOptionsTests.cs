using Raccoon.Extensions.OpenTelemetry.WebApi.Models;

namespace Raccoon.Extensions.OpenTelemetry.WebApi.Tests;

public class WebApiOpenTelemetryOptionsTests
{
    [Fact]
    public void Constructor_WhenCreated_SetsDefaultExcludedPaths()
    {
        // Arrange & Act
        var options = new WebApiOpenTelemetryOptions();

        // Assert
        Assert.Equal(["/health", "/alive"], options.ExcludedPaths);
    }

    [Fact]
    public void ExcludedPaths_WhenCustomPathAdded_ContainsDefaultsAndCustomPath()
    {
        // Arrange
        var options = new WebApiOpenTelemetryOptions();

        // Act
        options.ExcludedPaths.Add("/swagger");

        // Assert
        Assert.Equal(["/health", "/alive", "/swagger"], options.ExcludedPaths);
    }

    [Fact]
    public void ExcludedPaths_WhenReplacedViaInitializer_ContainsOnlyNewPaths()
    {
        // Arrange & Act
        var options = new WebApiOpenTelemetryOptions
        {
            ExcludedPaths = ["/custom-health"]
        };

        // Assert
        Assert.Equal(["/custom-health"], options.ExcludedPaths);
    }
}