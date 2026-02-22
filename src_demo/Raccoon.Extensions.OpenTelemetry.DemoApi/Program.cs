using Raccoon.Extensions.OpenTelemetry.Cosmos.Extensions;
using Raccoon.Extensions.OpenTelemetry.DemoApi.Extensions;
using Raccoon.Extensions.OpenTelemetry.EntityFrameworkCore.Extensions;
using Raccoon.Extensions.OpenTelemetry.Extensions;
using Raccoon.Extensions.OpenTelemetry.Npgsql.Extensions;
using Raccoon.Extensions.OpenTelemetry.SqlClient.Extensions;
using Raccoon.Extensions.OpenTelemetry.WebApi.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var otlpEndpoint = builder.Configuration["OpenTelemetry:OtlpEndpoint"];
ArgumentException.ThrowIfNullOrWhiteSpace(otlpEndpoint, "OpenTelemetry:OtlpEndpoint");

builder.AddOpenTelemetry("demo-api", o =>
{
    o.OtlpEndpoint = new Uri(otlpEndpoint);
    o.WithWebApi();
    o.WithNpgsql();
    o.WithSqlClient();
    o.WithEntityFrameworkCore();
    o.WithCosmos();
});

builder.Services.AddDemoServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();
app.Run();