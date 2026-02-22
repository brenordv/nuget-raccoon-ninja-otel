namespace Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Models;

public record CosmosContainerResponse(string Id, string PartitionKeyPath, int StatusCode);