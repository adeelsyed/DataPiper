using System;

namespace DataPiper
{
    public interface ILogService
    {
        void LogEventCompleted<U>(EventHandler<U> handler, string eventName) where U : EventArgs;
        void LogEventStarting<U>(EventHandler<U> handler, string eventName) where U : EventArgs;

        //wrap ILogger calls with null checking
        void LogTrace(string message, params object[] args);
        void LogTrace(Exception exception, string message, params object[] args);
        void LogDebug(string message, params object[] args);
        void LogDebug(Exception exception, string message, params object[] args);
        void LogInfo(string message, params object[] args);
        void LogInfo(Exception exception, string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogWarning(Exception exception, string message, params object[] args);
        void LogError(string message, params object[] args);
        void LogError(Exception exception, string message, params object[] args);
        void LogCritical(string message, params object[] args);
        void LogCritical(Exception exception, string message, params object[] args);
    }
}