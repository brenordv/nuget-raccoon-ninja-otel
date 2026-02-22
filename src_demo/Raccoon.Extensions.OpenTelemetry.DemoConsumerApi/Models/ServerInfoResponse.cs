namespace Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Models;

public record ServerInfoResponse(string MachineName, DateTimeOffset Timestamp, string OsDescription);