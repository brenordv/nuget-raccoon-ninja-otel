using Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Extensions;
using Raccoon.Extensions.OpenTelemetry.Extensions;
using Raccoon.Extensions.OpenTelemetry.WebApi.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var otlpEndpoint = builder.Configuration["OpenTelemetry:OtlpEndpoint"];
#pragma warning disable S3236 // Caller information arguments should not be provided explicitly -- config key is more meaningful than variable name
ArgumentException.ThrowIfNullOrWhiteSpace(otlpEndpoint, "OpenTelemetry:OtlpEndpoint");
#pragma warning restore S3236

builder.AddOpenTelemetry("demo-consumer-api", o =>
{
    o.OtlpEndpoint = new Uri(otlpEndpoint);
    o.WithWebApi();
});

builder.Services.AddRefitClients(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();
await app.RunAsync();