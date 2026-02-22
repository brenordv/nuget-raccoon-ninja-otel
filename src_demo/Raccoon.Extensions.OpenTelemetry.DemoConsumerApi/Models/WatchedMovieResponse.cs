using System.Text.Json.Serialization;

namespace Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Models;

public record WatchedMovieResponse(
    string LetterboxdUri,
    [property: JsonRequired] DateOnly WatchDate,
    string Title,
    [property: JsonRequired] short ReleaseYear,
    Guid? CacheId,
    string Genres);