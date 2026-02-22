using Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Clients;
using Refit;

namespace Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRefitClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var baseUrl = configuration["DemoApi:BaseUrl"];
#pragma warning disable S3236 // Caller information arguments should not be provided explicitly -- config key is more meaningful than variable name
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl, "DemoApi:BaseUrl");
#pragma warning restore S3236

        services
            .AddRefitClient<IDemoApiClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));

        return services;
    }
}