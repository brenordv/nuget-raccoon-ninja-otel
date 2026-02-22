# Raccoon.Extensions.OpenTelemetry

Drop-in OpenTelemetry setup for any .NET application. Configures tracing, metrics, and logging with a single line of code.

---
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-ninja-otel&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-ninja-otel)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-ninja-otel&metric=bugs)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-ninja-otel)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-ninja-otel&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-ninja-otel)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-ninja-otel&metric=coverage)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-ninja-otel)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-ninja-otel&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-ninja-otel)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-ninja-otel&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-ninja-otel)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-ninja-otel&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-ninja-otel)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-ninja-otel&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-ninja-otel)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-ninja-otel&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-ninja-otel)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-ninja-otel&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-ninja-otel)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-ninja-otel&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-ninja-otel)
---

## What to use?
- Regular CLI applications -> Raccoon.Extensions.OpenTelemetry
- ASP.NET Core applications -> Raccoon.Extensions.OpenTelemetry.WebApi
- Using Sql Server without EFCore -> Raccoon.Extensions.OpenTelemetry.SqlClient
- Using PostgreSQL without EFCore -> Raccoon.Extensions.OpenTelemetry.Npgsql
- Using Entity Framework Core -> Raccoon.Extensions.OpenTelemetry.EntityFrameworkCore
- Using Cosmos DB -> Raccoon.Extensions.OpenTelemetry.Cosmos

You can mix and match any of the above packages, and you can install just what you need, no need to install `Raccoon.Extensions.OpenTelemetry` + `other package`. 

## Experimental Dependencies
Before you start using all of these packages, it is important to point out that two extension packages rely on upstream
features that are **not yet stable**. Both are outside our control.

### Entity Framework Core

The `Raccoon.Extensions.OpenTelemetry.EntityFrameworkCore` package depends on
[`OpenTelemetry.Instrumentation.EntityFrameworkCore`](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.EntityFrameworkCore),
which is published **only as a prerelease** (`1.15.0-beta.1`) by the OpenTelemetry .NET project.
The upstream maintainers have not shipped a stable release yet, so breaking changes between beta versions are possible.

### Cosmos DB

The `Raccoon.Extensions.OpenTelemetry.Cosmos` package enables distributed tracing via the
`Azure.Experimental.EnableActivitySource` feature switch in the Azure SDK. Microsoft marks this as
**experimental** — the API surface, activity source names, or opt-in mechanism may change in future
Azure SDK releases.

Additionally, consumers must manually set `CosmosClientOptions.CosmosClientTelemetryOptions.DisableDistributedTracing = false`
on their `CosmosClient` for traces to be emitted.

> All other packages in this family depend exclusively on stable releases. We will update these extensions
> as soon as stable versions are available upstream.

## Quick Start

Install the core package:

```bash
dotnet add package Raccoon.Extensions.OpenTelemetry
```

### Web API

For ASP.NET Core apps, also install the WebApi extension:

```bash
dotnet add package Raccoon.Extensions.OpenTelemetry.WebApi
```

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddOpenTelemetry("my-api", o =>
{
    o.OtlpEndpoint = new Uri("http://localhost:4318");
    o.WithWebApi();
});
var app = builder.Build();
app.Run();
```

### CLI / Worker Service

For non-web apps, no extra packages are needed:

```csharp
var builder = Host.CreateApplicationBuilder(args);
builder.AddOpenTelemetry("my-cli", o =>
{
    o.OtlpEndpoint = new Uri("http://localhost:4318");
});
var host = builder.Build();
host.Run();
```

If `OtlpEndpoint` is not set, the exporter falls back to the `OTEL_EXPORTER_OTLP_ENDPOINT` environment variable:

```
OTEL_EXPORTER_OTLP_ENDPOINT=http://otel-collector:4318
```

That's it. All `ILogger<T>` calls, HttpClient spans, and runtime metrics are now exported via OTLP. Web APIs with `WithWebApi()` also get ASP.NET Core server spans and request metrics.

## What It Does

`AddOpenTelemetry()` configures the full OpenTelemetry pipeline:

- **Logging** - Hooks into `ILogger<T>` via `AddOpenTelemetry()` with formatted messages and scopes
- **Tracing** - HttpClient outgoing spans and custom `ActivitySource` spans
- **Metrics** - HttpClient metrics and .NET runtime metrics (GC, thread pool)
- **Export** - OTLP exporter for all three signals, using the endpoint and protocol from `OpenTelemetryOptions`

With `WithWebApi()`, ASP.NET Core server spans and request metrics are added. Health check endpoints (`/health`, `/alive`) are excluded from tracing by default.

## Packages

| Package                                                | Purpose                                             |
|--------------------------------------------------------|-----------------------------------------------------|
| `Raccoon.Extensions.OpenTelemetry`                     | Core - HttpClient tracing, runtime metrics, logging |
| `Raccoon.Extensions.OpenTelemetry.WebApi`              | ASP.NET Core tracing and metrics                    |
| `Raccoon.Extensions.OpenTelemetry.Npgsql`              | PostgreSQL tracing                                  |
| `Raccoon.Extensions.OpenTelemetry.SqlClient`           | SQL Server tracing and metrics                      |
| `Raccoon.Extensions.OpenTelemetry.EntityFrameworkCore` | Entity Framework Core tracing                       |
| `Raccoon.Extensions.OpenTelemetry.Cosmos`              | Azure Cosmos DB tracing                             |

## Usage Examples

### Web API with PostgreSQL

```bash
dotnet add package Raccoon.Extensions.OpenTelemetry
dotnet add package Raccoon.Extensions.OpenTelemetry.WebApi
dotnet add package Raccoon.Extensions.OpenTelemetry.Npgsql
```

```csharp
builder.AddOpenTelemetry("my-pg-api", o =>
{
    o.OtlpEndpoint = new Uri("http://localhost:4317");
    o.OtlpProtocol = OtlpExportProtocol.Grpc;
    o.WithWebApi();
    o.WithNpgsql();
});
```

### Web API with SQL Server + EF Core

```bash
dotnet add package Raccoon.Extensions.OpenTelemetry
dotnet add package Raccoon.Extensions.OpenTelemetry.WebApi
dotnet add package Raccoon.Extensions.OpenTelemetry.SqlClient
dotnet add package Raccoon.Extensions.OpenTelemetry.EntityFrameworkCore
```

```csharp
builder.AddOpenTelemetry("my-sql-api", o =>
{
    o.OtlpEndpoint = new Uri("http://localhost:4318");
    o.WithWebApi();
    o.WithSqlClient();
    o.WithEntityFrameworkCore();
});
```

### Web API with Cosmos DB

```bash
dotnet add package Raccoon.Extensions.OpenTelemetry
dotnet add package Raccoon.Extensions.OpenTelemetry.WebApi
dotnet add package Raccoon.Extensions.OpenTelemetry.Cosmos
```

```csharp
builder.AddOpenTelemetry("my-cosmos-api", o =>
{
    o.OtlpEndpoint = new Uri("http://localhost:4318");
    o.WithWebApi();
    o.WithCosmos();
});
```

> Cosmos DB also requires setting `CosmosClientOptions.CosmosClientTelemetryOptions.DisableDistributedTracing = false` on your `CosmosClient`.

### CLI with PostgreSQL

```bash
dotnet add package Raccoon.Extensions.OpenTelemetry
dotnet add package Raccoon.Extensions.OpenTelemetry.Npgsql
```

```csharp
builder.AddOpenTelemetry("my-cli", o =>
{
    o.OtlpEndpoint = new Uri("http://localhost:4318");
    o.WithNpgsql();
});
```

### With Customization

```csharp
builder.AddOpenTelemetry("my-api", o =>
{
    o.OtlpEndpoint = new Uri("http://localhost:4318");
    o.WithWebApi(webApi => webApi.ExcludedPaths.Add("/swagger"));
    o.WithNpgsql();
    o.ResourceAttributes["deployment.environment"] = "production";
    o.ConfigureTracing = tracing =>
        tracing.SetSampler(new TraceIdRatioBasedSampler(0.5));
});
```

## Custom Spans

Custom spans use native .NET `ActivitySource` - no wrapper needed:

```csharp
private static readonly ActivitySource Source = new("my-api");

public async Task ProcessOrderAsync(int orderId)
{
    using var activity = Source.StartActivity("ProcessOrder");
    activity?.SetTag("order.id", orderId);
    _logger.LogInformation("Processing order {OrderId}", orderId);
}
```

## Configuration Options

### Core (`OpenTelemetryOptions`)

| Option                | Default        | Description                                                                            |
|-----------------------|----------------|----------------------------------------------------------------------------------------|
| `OtlpEndpoint`        | `null`         | OTLP collector endpoint; falls back to `OTEL_EXPORTER_OTLP_ENDPOINT` env var when null |
| `OtlpProtocol`        | `HttpProtobuf` | OTLP export protocol (`HttpProtobuf` for port 4318, `Grpc` for port 4317)              |
| `AdditionalSources`   | `[]`           | Extra `ActivitySource` names to capture                                                |
| `ResourceAttributes`  | `{}`           | Extra OTel resource attributes                                                         |
| `ConfigureTracing`    | `null`         | Escape hatch for advanced tracing config                                               |
| `ConfigureMetrics`    | `null`         | Escape hatch for advanced metrics config                                               |
| `TracingContributors` | `[]`           | Tracing contributors registered by extension packages via `WithXxx()` methods          |
| `MetricsContributors` | `[]`           | Metrics contributors registered by extension packages via `WithXxx()` methods          |

### WebApi (`WebApiOpenTelemetryOptions`)

| Option          | Default                 | Description                         |
|-----------------|-------------------------|-------------------------------------|
| `ExcludedPaths` | `["/health", "/alive"]` | Request paths excluded from tracing |

## Requirements

- .NET 10
- An OTLP-compatible collector (Jaeger, Aspire Dashboard, Grafana Alloy, OpenTelemetry Collector, etc.)

## Important Notes

- `UseOtlpExporter()` is used for all signals. This **cannot coexist** with per-signal `AddOtlpExporter()` calls. This package owns the full OTel pipeline - don't mix with manual OTel configuration.
- The OTLP endpoint can be set via `OpenTelemetryOptions.OtlpEndpoint` or the `OTEL_EXPORTER_OTLP_ENDPOINT` environment variable. When both are set, `OtlpEndpoint` takes precedence.
- All existing `ILogger<T>` calls automatically get exported - no code changes needed in your services.

## License

MIT
