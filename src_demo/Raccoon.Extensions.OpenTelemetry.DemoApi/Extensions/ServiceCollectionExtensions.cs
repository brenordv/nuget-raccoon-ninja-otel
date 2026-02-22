using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Raccoon.Extensions.OpenTelemetry.DemoApi.Data;

namespace Raccoon.Extensions.OpenTelemetry.DemoApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDemoServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var postgresCs = configuration.GetConnectionString("PostgreSql");
        if (!string.IsNullOrWhiteSpace(postgresCs))
        {
            services.AddSingleton(NpgsqlDataSource.Create(postgresCs));
        }

        var sqlServerCs = configuration.GetConnectionString("SqlServer");
        if (!string.IsNullOrWhiteSpace(sqlServerCs))
        {
            services.AddDbContext<DemoDbContext>(o => o.UseSqlServer(sqlServerCs));
        }

        var cosmosCs = configuration.GetConnectionString("CosmosDb");
        if (!string.IsNullOrWhiteSpace(cosmosCs))
        {
            services.AddSingleton(_ => new CosmosClient(cosmosCs));
        }

        services.AddHttpClient("demo");

        return services;
    }
}