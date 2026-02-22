namespace Raccoon.Extensions.OpenTelemetry.DemoConsumerApi.Models;

public record WatchedMovieResponse(
    string LetterboxdUri,
    DateOnly WatchDate,
    string Title,
    short ReleaseYear,
    Guid? CacheId,
    string Genres);