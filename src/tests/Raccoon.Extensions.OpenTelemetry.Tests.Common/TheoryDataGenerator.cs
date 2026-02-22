namespace Raccoon.Extensions.OpenTelemetry.Tests.Common;

/// <summary>
/// Centralized TheoryData generators for shared test data across test projects.
/// </summary>
public static class TheoryDataGenerator
{
    /// <summary>
    /// Provides empty and whitespace string values for testing input validation.
    /// </summary>
    /// <returns>A <see cref="TheoryData{T}"/> containing empty and whitespace strings.</returns>
    public static TheoryData<string> EmptyOrWhitespaceStrings() =>
    [
        "",
        "   "
    ];

    /// <summary>
    /// Provides strings that are not valid URIs for testing URI parsing validation.
    /// </summary>
    /// <returns>A <see cref="TheoryData{T}"/> containing invalid URI strings.</returns>
    public static TheoryData<string> InvalidUriStrings() =>
    [
        "not-a-uri",
        "://missing-scheme",
        "http://",
        "just some text"
    ];
}