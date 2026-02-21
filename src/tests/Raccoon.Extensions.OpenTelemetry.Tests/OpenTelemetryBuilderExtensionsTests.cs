using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Raccoon.Extensions.OpenTelemetry.Extensions;
using Raccoon.Extensions.OpenTelemetry.Tests.Common;

namespace Raccoon.Extensions.OpenTelemetry.Tests;

public class OpenTelemetryBuilderExtensionsTests
{
    [Fact]
    public void AddOpenTelemetry_WithNullServiceName_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddOpenTelemetry(null!));
    }

    [Theory]
    [MemberData(nameof(TheoryDataGenerator.EmptyOrWhitespaceStrings), MemberType = typeof(TheoryDataGenerator))]
    public void AddOpenTelemetry_WithEmptyOrWhitespaceServiceName_ThrowsArgumentException(string serviceName)
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.AddOpenTelemetry(serviceName));
    }

    [Fact]
    public void AddOpenTelemetry_WithValidServiceName_ReturnsSameBuilder()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        // Act
        var result = builder.AddOpenTelemetry("test-service");

        // Assert
        Assert.Same(builder, result);
    }

    [Fact]
    public void AddOpenTelemetry_WhenCalled_RegistersOpenTelemetryServices()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        // Act
        builder.AddOpenTelemetry("test-service");

        // Assert
        var serviceProvider = builder.Services.BuildServiceProvider();
        var tracerProvider = serviceProvider.GetService<TracerProvider>();
        var meterProvider = serviceProvider.GetService<MeterProvider>();
        Assert.NotNull(tracerProvider);
        Assert.NotNull(meterProvider);
    }

    [Fact]
    public void AddOpenTelemetry_WithConfigureCallback_InvokesCallback()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        var callbackInvoked = false;

        // Act
        builder.AddOpenTelemetry("test-service", options =>
        {
            callbackInvoked = true;
        });

        // Assert
        Assert.True(callbackInvoked);
    }

    [Fact]
    public void AddOpenTelemetry_WithNullConfigure_DoesNotThrow()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        // Act
        var exception = Record.Exception(() => builder.AddOpenTelemetry("test-service", configure: null));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void AddOpenTelemetry_WithTracingContributor_ContributorIsInvoked()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        var contributorInvoked = false;

        // Act
        builder.AddOpenTelemetry("test-service", options =>
        {
            options.TracingContributors.Add(_ => contributorInvoked = true);
        });

        var serviceProvider = builder.Services.BuildServiceProvider();
        _ = serviceProvider.GetService<TracerProvider>();

        // Assert
        Assert.True(contributorInvoked);
    }

    [Fact]
    public void AddOpenTelemetry_WithMetricsContributor_ContributorIsInvoked()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        var contributorInvoked = false;

        // Act
        builder.AddOpenTelemetry("test-service", options =>
        {
            options.MetricsContributors.Add(_ => contributorInvoked = true);
        });

        var serviceProvider = builder.Services.BuildServiceProvider();
        _ = serviceProvider.GetService<MeterProvider>();

        // Assert
        Assert.True(contributorInvoked);
    }

    [Fact]
    public void AddOpenTelemetry_WithConfigureTracing_EscapeHatchIsInvoked()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        var configureInvoked = false;

        // Act
        builder.AddOpenTelemetry("test-service", options =>
        {
            options.ConfigureTracing = _ => configureInvoked = true;
        });

        var serviceProvider = builder.Services.BuildServiceProvider();
        _ = serviceProvider.GetService<TracerProvider>();

        // Assert
        Assert.True(configureInvoked);
    }

    [Fact]
    public void AddOpenTelemetry_WithConfigureMetrics_EscapeHatchIsInvoked()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();
        var configureInvoked = false;

        // Act
        builder.AddOpenTelemetry("test-service", options =>
        {
            options.ConfigureMetrics = _ => configureInvoked = true;
        });

        var serviceProvider = builder.Services.BuildServiceProvider();
        _ = serviceProvider.GetService<MeterProvider>();

        // Assert
        Assert.True(configureInvoked);
    }

    [Fact]
    public void AddOpenTelemetry_WithAdditionalSources_DoesNotThrow()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        // Act
        var exception = Record.Exception(() => builder.AddOpenTelemetry("test-service", options =>
        {
            options.AdditionalSources.Add("CustomSource");
        }));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void AddOpenTelemetry_WithResourceAttributes_DoesNotThrow()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        // Act
        var exception = Record.Exception(() => builder.AddOpenTelemetry("test-service", options =>
        {
            options.ResourceAttributes["deployment.environment"] = "test";
        }));

        // Assert
        Assert.Null(exception);
    }
}