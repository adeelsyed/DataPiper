using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace DataPiper
{
    public class LogService : ILogService
    {
        private readonly ILogger<LogService> _logger;

        public LogService(ILogger<LogService> logger = null)
        {
            _logger = logger;
        }

        public void LogEventStarting<U>(EventHandler<U> handler, string eventName) where U : EventArgs
        {
            _logger?.LogDebug("Starting {eventName}. Invocation list: {list}",
                eventName,
                string.Join("; ", handler.GetInvocationList().Select(d => d.Method)));
        }
        public void LogEventCompleted<U>(EventHandler<U> handler, string eventName) where U : EventArgs
        {
            _logger?.LogDebug("Completed {eventName}", eventName);
        }

        //wrap ILogger calls with null checking
        public void LogTrace(string message, params object[] args) => _logger?.LogTrace(message, args);
        public void LogTrace(Exception exception, string message, params object[] args) => _logger?.LogTrace(exception, message, args);
        public void LogDebug(string message, params object[] args) => _logger?.LogDebug(message, args);
        public void LogDebug(Exception exception, string message, params object[] args) => _logger?.LogDebug(exception, message, args);
        public void LogInfo(string message, params object[] args) => _logger?.LogInformation(message, args);
        public void LogInfo(Exception exception, string message, params object[] args) => _logger?.LogInformation(exception, message, args);
        public void LogWarning(string message, params object[] args) => _logger?.LogWarning(message, args);
        public void LogWarning(Exception exception, string message, params object[] args) => _logger?.LogWarning(exception, message, args);
        public void LogError(string message, params object[] args) => _logger?.LogError(message, args);
        public void LogError(Exception exception, string message, params object[] args) => _logger?.LogError(exception, message, args);
        public void LogCritical(string message, params object[] args) => _logger?.LogCritical(message, args);
        public void LogCritical(Exception exception, string message, params object[] args) => _logger?.LogCritical(exception, message, args);

    }
}
