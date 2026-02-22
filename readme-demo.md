# Demo Applications

Two demo APIs showcase `Raccoon.Extensions.OpenTelemetry` in action. The **Demo API** talks directly to
databases and external services. The **Consumer API** proxies every call through the Demo API via Refit,
demonstrating that OpenTelemetry auto-instrumentation propagates trace context across HTTP boundaries
without any manual effort.

When both are running together, a single request to the Consumer API produces a distributed trace that
spans: **Consumer API -> Demo API -> Database / External Service**.

## Prerequisites

- .NET 10 SDK
- An OTLP-compatible collector (e.g. [Jaeger](https://www.jaegertracing.io/), [Aspire Dashboard](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard/overview), or [Grafana Tempo](https://grafana.com/oss/tempo/))
- One or more of the following (all are optional; each endpoint degrades gracefully if its backing service is unavailable):
  - PostgreSQL instance
  - SQL Server instance
  - Azure Cosmos DB account or the [Cosmos DB Emulator](https://learn.microsoft.com/en-us/azure/cosmos-db/emulator)

## Configuration

Both projects read settings from `appsettings.json`. Copy `appsettings.example.json` to
`appsettings.json` in each project and fill in the values.

### Demo API (`src_demo/Raccoon.Extensions.OpenTelemetry.DemoApi`)

| Key                            | Required | Description                                                                     |
|--------------------------------|----------|---------------------------------------------------------------------------------|
| `OpenTelemetry:OtlpEndpoint`   | Yes      | OTLP collector gRPC endpoint (e.g. `http://localhost:4317`)                     |
| `ConnectionStrings:PostgreSql` | No       | PostgreSQL connection string                                                    |
| `ConnectionStrings:SqlServer`  | No       | SQL Server connection string (used by both the SqlClient and EF Core endpoints) |
| `ConnectionStrings:CosmosDb`   | No       | Azure Cosmos DB connection string                                               |
| `Cosmos:DatabaseName`          | No       | Cosmos DB database name (required if `CosmosDb` connection string is set)       |
| `Cosmos:ContainerName`         | No       | Cosmos DB container name (required if `CosmosDb` connection string is set)      |

### Consumer API (`src_demo/Raccoon.Extensions.OpenTelemetry.DemoConsumerApi`)

| Key                          | Required | Description                                                 |
|------------------------------|----------|-------------------------------------------------------------|
| `OpenTelemetry:OtlpEndpoint` | Yes      | OTLP collector gRPC endpoint (e.g. `http://localhost:4317`) |
| `DemoApi:BaseUrl`            | Yes      | Base URL of the Demo API (e.g. `http://localhost:5196`)     |

## Running the Demo

Start the Demo API first, then the Consumer API:

```bash
# Terminal 1 - Demo API (port 5196)
dotnet run --project src_demo/Raccoon.Extensions.OpenTelemetry.DemoApi

# Terminal 2 - Consumer API (port 5197)
dotnet run --project src_demo/Raccoon.Extensions.OpenTelemetry.DemoConsumerApi
```

Both APIs expose a Scalar UI for interactive testing:

- Demo API: http://localhost:5196/scalar/v1
- Consumer API: http://localhost:5197/scalar/v1

## Endpoints

Both APIs expose the same routes. The Demo API handles each request directly, while the Consumer API
proxies every call to the Demo API through a Refit HTTP client.

| Method | Route                 | Demo API Behavior                                                                  | Consumer API Behavior |
|--------|-----------------------|------------------------------------------------------------------------------------|-----------------------|
| `GET`  | `/api/demo/webapi`    | Returns machine name, UTC timestamp, and OS description                            | Proxies to Demo API   |
| `GET`  | `/api/demo/http`      | Sends an outbound GET to `https://httpbin.org/get` and returns the response        | Proxies to Demo API   |
| `GET`  | `/api/demo/postgres`  | Runs `SELECT version()` via Dapper against PostgreSQL                              | Proxies to Demo API   |
| `GET`  | `/api/demo/sqlserver` | Runs `SELECT @@VERSION` via `Microsoft.Data.SqlClient` against SQL Server          | Proxies to Demo API   |
| `GET`  | `/api/demo/efcore`    | Queries the `watched` table via EF Core; returns `200` with data or `204` if empty | Proxies to Demo API   |
| `POST` | `/api/demo/efcore`    | Inserts a new row into the `watched` table via EF Core; returns `201`              | Proxies to Demo API   |
| `GET`  | `/api/demo/cosmos`    | Reads Cosmos DB container properties (ID, partition key path, status code)         | Proxies to Demo API   |

## OTel Packages Used

| API          | Packages                                                     | Why                                                                                                                                           |
|--------------|--------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------|
| Demo API     | Core, WebApi, Npgsql, SqlClient, EntityFrameworkCore, Cosmos | Direct access to all supported data sources                                                                                                   |
| Consumer API | Core, WebApi                                                 | Only needs ASP.NET Core + HttpClient instrumentation (Refit calls are traced via the built-in HttpClient instrumentation in the Core package) |

## Database Setup

The EF Core and PostgreSQL endpoints require a `watched` table. Use the scripts below to create it and
insert sample data.

### PostgreSQL

```sql
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS watched (
    letterboxd_uri   TEXT      NOT NULL,
    watch_date       DATE      NOT NULL,
    title            TEXT      NOT NULL,
    release_year     SMALLINT  NOT NULL
        CHECK (release_year BETWEEN 1878 AND 2100),
    cache_id         UUID      NULL,
    genres           TEXT[]    NULL,

    CONSTRAINT letterboxd_watchlist_pk
        PRIMARY KEY (letterboxd_uri)
);
```

```sql
INSERT INTO watched (letterboxd_uri, watch_date, title, release_year, genres, cache_id)
VALUES
    ('/film/the-shawshank-redemption/', '2025-01-15', 'The Shawshank Redemption', 1994, ARRAY['Drama','Crime'], 'a1b2c3d4-e5f6-7890-abcd-ef1234567890'),
    ('/film/parasite-2019/', '2025-02-03', 'Parasite', 2019, ARRAY['Thriller','Drama','Comedy'], 'b2c3d4e5-f6a7-8901-bcde-f12345678901'),
    ('/film/spirited-away/', '2025-02-10', 'Spirited Away', 2001, ARRAY['Animation','Fantasy','Adventure'], 'c3d4e5f6-a7b8-9012-cdef-123456789012'),
    ('/film/the-godfather/', '2025-01-28', 'The Godfather', 1972, ARRAY['Crime','Drama'], 'd4e5f6a7-b8c9-0123-defa-234567890123'),
    ('/film/everything-everywhere-all-at-once/', '2025-02-18', 'Everything Everywhere All at Once', 2022, ARRAY['Action','Adventure','Comedy','Sci-Fi'], 'e5f6a7b8-c9d0-1234-efab-345678901234');
```

### SQL Server

```sql
IF NOT EXISTS (
    SELECT * FROM sys.tables WHERE name = 'watched'
)
CREATE TABLE watched (
    letterboxd_uri   NVARCHAR(200)        NOT NULL,
    watch_date       DATE                 NOT NULL,
    title            NVARCHAR(MAX)        NOT NULL,
    release_year     SMALLINT             NOT NULL
        CHECK (release_year BETWEEN 1878 AND 2100),
    cache_id         UNIQUEIDENTIFIER     NULL,
    genres           NVARCHAR(MAX)        NULL,

    CONSTRAINT letterboxd_watchlist_pk
        PRIMARY KEY (letterboxd_uri)
);
```

```sql
INSERT INTO watched (letterboxd_uri, watch_date, title, release_year, genres, cache_id)
VALUES
    ('/film/the-shawshank-redemption/', '2025-01-15', 'The Shawshank Redemption', 1994, '["Drama","Crime"]', 'a1b2c3d4-e5f6-7890-abcd-ef1234567890'),
    ('/film/parasite-2019/', '2025-02-03', 'Parasite', 2019, '["Thriller","Drama","Comedy"]', 'b2c3d4e5-f6a7-8901-bcde-f12345678901'),
    ('/film/spirited-away/', '2025-02-10', 'Spirited Away', 2001, '["Animation","Fantasy","Adventure"]', 'c3d4e5f6-a7b8-9012-cdef-123456789012'),
    ('/film/the-godfather/', '2025-01-28', 'The Godfather', 1972, '["Crime","Drama"]', 'd4e5f6a7-b8c9-0123-defa-234567890123'),
    ('/film/everything-everywhere-all-at-once/', '2025-02-18', 'Everything Everywhere All at Once', 2022, '["Action","Adventure","Comedy","Sci-Fi"]', 'e5f6a7b8-c9d0-1234-efab-345678901234');
```

## Example: Inserting a Movie via the EF Core Endpoint

```bash
curl -X POST http://localhost:5196/api/demo/efcore \
  -H "Content-Type: application/json" \
  -d '{
    "letterboxdUri": "/film/arrival-2016/",
    "watchDate": "2025-03-01",
    "title": "Arrival",
    "releaseYear": 2016,
    "cacheId": null,
    "genres": "[\"Drama\",\"Sci-Fi\"]"
  }'
```

Or through the Consumer API (same payload, different port):

```bash
curl -X POST http://localhost:5197/api/demo/efcore \
  -H "Content-Type: application/json" \
  -d '{
    "letterboxdUri": "/film/arrival-2016/",
    "watchDate": "2025-03-01",
    "title": "Arrival",
    "releaseYear": 2016,
    "cacheId": null,
    "genres": "[\"Drama\",\"Sci-Fi\"]"
  }'
```

## Verifying Traces

After making requests, open your OTLP collector's UI and look for traces with the service names
`demo-api` and `demo-consumer-api`. When calling the Consumer API, you should see a trace that
includes spans from both services, proving that the W3C `traceparent` header is automatically
propagated across HTTP boundaries.