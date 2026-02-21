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
    public static TheoryData<string> EmptyOrWhitespaceStrings() => new()
    {
        { "" },
        { "   " },
    };
}