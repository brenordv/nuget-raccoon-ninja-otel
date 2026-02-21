using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Raccoon.Extensions.OpenTelemetry.Tests.Common.Validators;

/// <summary>
/// Extension methods for verifying <see cref="ILogger{T}"/> interactions in unit tests.
/// </summary>
public static class LoggingValidator
{
    /// <summary>
    /// Verifies that the <see cref="ILogger{T}"/> log method was called with the specified log level, message, and exception parameters.
    /// </summary>
    /// <param name="logger">The NSubstitute mock of <see cref="ILogger{T}"/> to verify.</param>
    /// <param name="times">The expected number of times the log method was called.</param>
    /// <param name="level">The expected <see cref="LogLevel"/>.</param>
    /// <param name="message">The expected log message fragment, or <c>null</c> to skip message verification.</param>
    /// <typeparam name="T">The category type of the logger.</typeparam>
    public static void VerifyLogging<T>(this ILogger<T> logger, int times, LogLevel level, string message = null)
    {
        logger.Received(times).Log(
            Arg.Is<LogLevel>(o => o == level),
            Arg.Any<EventId>(),
            Arg.Is<object>(o => times == 0 || o.CheckMessage(message)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());
    }

    /// <summary>
    /// Checks if the message received by the logger contains the expected message.
    /// </summary>
    /// <param name="obj">The log state object to inspect.</param>
    /// <param name="expectedMessage">The expected message fragment to match against.</param>
    /// <returns><c>true</c> if the message matches or no expected message was provided; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// If the log verification is failing, but you think it should be passing, place a breakpoint
    /// in this method and compare the values in <c>objAsStr</c> and <c>expectedMessage</c>.
    /// </remarks>
    private static bool CheckMessage(this object obj, string expectedMessage)
    {
        // When we're not checking for the log message, we can return true.
        if (string.IsNullOrEmpty(expectedMessage))
        {
            return true;
        }

        var objAsStr = obj.ToString();
        return objAsStr is not null && (expectedMessage.Contains(objAsStr) || objAsStr.Contains(expectedMessage));
    }
}