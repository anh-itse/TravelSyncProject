using Microsoft.Extensions.Logging;

namespace TravelSync.Shared.Helpers;

public static partial class LogHelper
{
    /// <summary>
    /// Hiển thị log infomation
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    [LoggerMessage(1001, LogLevel.Information, "[AuditLog] {Message}")]
    public static partial void Info(ILogger logger, string message);

    /// <summary>
    /// Hiển thị log Error
    /// </summary>
    /// <param name="logger">ILogger.</param>
    /// <param name="ex">Exception.</param>
    /// <param name="requestName">RequestName</param>
    /// <param name="errorMessage">ErrorMessage</param>
    [LoggerMessage(1003, LogLevel.Error, "[AuditLog] {ErrorMessage}")]
    public static partial void Error(ILogger logger, Exception ex, string errorMessage);
}
