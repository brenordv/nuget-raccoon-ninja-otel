using Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Models;
using Refit;

namespace Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Clients;

public interface IDemoApiClient
{
    [Get("/api/demo/webapi")]
    Task<ApiResponse<ServerInfoResponse>> GetServerInfoAsync(CancellationToken cancellationToken = default);

    [Get("/api/demo/http")]
    Task<ApiResponse<HttpBinResponse>> GetHttpBinAsync(CancellationToken cancellationToken = default);

    [Get("/api/demo/postgres")]
    Task<ApiResponse<VersionResponse>> GetPostgresVersionAsync(CancellationToken cancellationToken = default);

    [Get("/api/demo/sqlserver")]
    Task<ApiResponse<VersionResponse>> GetSqlServerVersionAsync(CancellationToken cancellationToken = default);

    [Get("/api/demo/efcore")]
    Task<ApiResponse<List<WatchedMovieResponse>>> GetWatchedMoviesAsync(CancellationToken cancellationToken = default);

    [Post("/api/demo/efcore")]
    Task<ApiResponse<WatchedMovieResponse>> PostWatchedMovieAsync(
        [Body] WatchedMovieResponse movie,
        CancellationToken cancellationToken = default);

    [Get("/api/demo/cosmos")]
    Task<ApiResponse<CosmosContainerResponse>> GetCosmosContainerAsync(CancellationToken cancellationToken = default);
}