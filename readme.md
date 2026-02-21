# Raccoon.Extensions.OpenTelemetry

Drop-in OpenTelemetry setup for any .NET application. Configures tracing, metrics, and logging with a single line of code.

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
builder.AddRaccoonOtel("my-api", o => o.WithWebApi());
var app = builder.Build();
app.Run();
```

### CLI / Worker Service

For non-web apps, no extra packages are needed:

```csharp
var builder = Host.CreateApplicationBuilder(args);
builder.AddRaccoonOtel("my-cli");
var host = builder.Build();
host.Run();
```

Set the OTLP endpoint via environment variable:

```
OTEL_EXPORTER_OTLP_ENDPOINT=http://otel-collector:4318
```

That's it. All `ILogger<T>` calls, HttpClient spans, and runtime metrics are now exported via OTLP. Web APIs with `WithWebApi()` also get ASP.NET Core server spans and request metrics.

## What It Does

`AddRaccoonOtel()` configures the full OpenTelemetry pipeline:

- **Logging** - Hooks into `ILogger<T>` via `AddOpenTelemetry()` with formatted messages and scopes
- **Tracing** - HttpClient outgoing spans and custom `ActivitySource` spans
- **Metrics** - HttpClient metrics and .NET runtime metrics (GC, thread pool)
- **Export** - OTLP exporter for all three signals, configured via `OTEL_EXPORTER_OTLP_ENDPOINT`

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
builder.AddRaccoonOtel("my-pg-api", o =>
{
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
builder.AddRaccoonOtel("my-sql-api", o =>
{
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
builder.AddRaccoonOtel("my-cosmos-api", o =>
{
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
builder.AddRaccoonOtel("my-cli", o => o.WithNpgsql());
```

### With Customization

```csharp
builder.AddRaccoonOtel("my-api", o =>
{
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

### Core (`RaccoonOtelOptions`)

| Option               | Default | Description                              |
|----------------------|---------|------------------------------------------|
| `AdditionalSources`  | `[]`    | Extra `ActivitySource` names to capture  |
| `ResourceAttributes` | `{}`    | Extra OTel resource attributes           |
| `ConfigureTracing`   | `null`  | Escape hatch for advanced tracing config |
| `ConfigureMetrics`   | `null`  | Escape hatch for advanced metrics config |

### WebApi (`WebApiOtelOptions`)

| Option          | Default                 | Description                         |
|-----------------|-------------------------|-------------------------------------|
| `ExcludedPaths` | `["/health", "/alive"]` | Request paths excluded from tracing |

## Requirements

- .NET 10
- An OTLP-compatible collector (Grafana Alloy, OpenTelemetry Collector, etc.)

## Important Notes

- `UseOtlpExporter()` is used for all signals. This **cannot coexist** with per-signal `AddOtlpExporter()` calls. This package owns the full OTel pipeline - don't mix with manual OTel configuration.
- The OTLP endpoint is configured exclusively via the `OTEL_EXPORTER_OTLP_ENDPOINT` environment variable.
- All existing `ILogger<T>` calls automatically get exported - no code changes needed in your services.

## License

MIT
