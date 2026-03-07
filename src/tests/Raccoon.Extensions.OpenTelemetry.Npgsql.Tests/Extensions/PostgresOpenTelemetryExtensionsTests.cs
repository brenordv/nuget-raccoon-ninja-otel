using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Raccoon.Extensions.OpenTelemetry.Models;
using Raccoon.Extensions.OpenTelemetry.Npgsql.Extensions;

namespace Raccoon.Extensions.OpenTelemetry.Npgsql.Tests.Extensions;

public sealed class PostgresOpenTelemetryExtensionsTests : IDisposable
{
    private const string NpgsqlSourceName = "Npgsql";
    private const string NonNpgsqlSourceName = "SomeOtherSource";
    private const string PeerServiceKey = "peer.service";
    private const string DbSystemKey = "db.system";
    private const string ExpectedPeerServiceValue = "postgresql";
    private const string ExpectedDbSystemValue = "postgresql";

    private readonly ActivitySource _npgsqlSource = new(NpgsqlSourceName);
    private readonly ActivitySource _otherSource = new(NonNpgsqlSourceName);

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

    [Fact]
    public void WithNpgsql_WhenNpgsqlActivityCompletes_AddsPeerServiceTag()
    {
        // Arrange
        var exportedActivities = new List<Activity>();
        using var tracerProvider = BuildTracerProvider(exportedActivities);

        // Act
        using (var activity = _npgsqlSource.StartActivity("SELECT version()", ActivityKind.Client))
        {
            activity?.SetTag("db.system.name", "postgresql");
        }

        // Assert
        var exported = Assert.Single(exportedActivities);
        Assert.Equal(ExpectedPeerServiceValue, exported.GetTagItem(PeerServiceKey));
    }

    [Fact]
    public void WithNpgsql_WhenNpgsqlActivityCompletes_AddsDbSystemTag()
    {
        // Arrange
        var exportedActivities = new List<Activity>();
        using var tracerProvider = BuildTracerProvider(exportedActivities);

        // Act
        using (var activity = _npgsqlSource.StartActivity("SELECT version()", ActivityKind.Client))
        {
            activity?.SetTag("db.system.name", "postgresql");
        }

        // Assert
        var exported = Assert.Single(exportedActivities);
        Assert.Equal(ExpectedDbSystemValue, exported.GetTagItem(DbSystemKey));
    }

    [Fact]
    public void WithNpgsql_WhenNonNpgsqlActivityCompletes_DoesNotAddTags()
    {
        // Arrange
        var exportedActivities = new List<Activity>();
        using var tracerProvider = BuildTracerProvider(exportedActivities, NonNpgsqlSourceName);

        // Act
        using (_otherSource.StartActivity("some-operation", ActivityKind.Client))
        {
        }

        // Assert
        var exported = Assert.Single(exportedActivities);
        Assert.Null(exported.GetTagItem(PeerServiceKey));
        Assert.Null(exported.GetTagItem(DbSystemKey));
    }

    [Fact]
    public void WithNpgsql_WhenNpgsqlActivityCompletes_PreservesExistingTags()
    {
        // Arrange
        var exportedActivities = new List<Activity>();
        using var tracerProvider = BuildTracerProvider(exportedActivities);

        // Act
        using (var activity = _npgsqlSource.StartActivity("SELECT version()", ActivityKind.Client))
        {
            activity?.SetTag("db.system.name", "postgresql");
            activity?.SetTag("server.address", "rverse.local");
        }

        // Assert
        var exported = Assert.Single(exportedActivities);
        Assert.Equal("postgresql", exported.GetTagItem("db.system.name"));
        Assert.Equal("rverse.local", exported.GetTagItem("server.address"));
        Assert.Equal(ExpectedPeerServiceValue, exported.GetTagItem(PeerServiceKey));
    }

    [Fact]
    public void WithNpgsql_WhenPeerServiceAlreadySet_DoesNotOverwrite()
    {
        // Arrange
        var exportedActivities = new List<Activity>();
        using var tracerProvider = BuildTracerProvider(exportedActivities);
        const string customPeerService = "postgresql-primary";

        // Act
        using (var activity = _npgsqlSource.StartActivity("SELECT version()", ActivityKind.Client))
        {
            activity?.SetTag(PeerServiceKey, customPeerService);
        }

        // Assert
        var exported = Assert.Single(exportedActivities);
        Assert.Equal(customPeerService, exported.GetTagItem(PeerServiceKey));
    }

    [Fact]
    public void WithNpgsql_WhenDbSystemAlreadySet_DoesNotOverwrite()
    {
        // Arrange
        var exportedActivities = new List<Activity>();
        using var tracerProvider = BuildTracerProvider(exportedActivities);
        const string customDbSystem = "cockroachdb";

        // Act
        using (var activity = _npgsqlSource.StartActivity("SELECT version()", ActivityKind.Client))
        {
            activity?.SetTag(DbSystemKey, customDbSystem);
        }

        // Assert
        var exported = Assert.Single(exportedActivities);
        Assert.Equal(customDbSystem, exported.GetTagItem(DbSystemKey));
    }

    public void Dispose()
    {
        _npgsqlSource.Dispose();
        _otherSource.Dispose();
    }

    #region Test Helpers

    private static TracerProvider BuildTracerProvider(
        List<Activity> exportedActivities,
        string additionalSource = null)
    {
        var options = new OpenTelemetryOptions();
        options.WithNpgsql();

        var builder = Sdk.CreateTracerProviderBuilder()
            .AddSource(NpgsqlSourceName);

        if (additionalSource is not null)
        {
            builder.AddSource(additionalSource);
        }

        foreach (var contributor in options.TracingContributors)
        {
            contributor(builder);
        }

        builder.AddInMemoryExporter(exportedActivities);

        return builder.Build();
    }

    #endregion
}