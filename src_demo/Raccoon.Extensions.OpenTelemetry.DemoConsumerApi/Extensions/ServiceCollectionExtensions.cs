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
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl, "DemoApi:BaseUrl");

        services
            .AddRefitClient<IDemoApiClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));

        return services;
    }
}