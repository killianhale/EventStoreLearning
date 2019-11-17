using System;
using System.Collections.Generic;
using NLog;

namespace EventStoreLearning.Common.Logging
{
    public static class LoggingExtensions
    {
        public static void LogWithContext(this ILogger logger, LogLevel level, string message, object context)
        {
            var e = new LogEventInfo(level, logger.Name, message);
            e.Properties.Add("context", context);

            logger.Log(e);
        }

        public static void DebugWithContext(this ILogger logger, string message, object context)
        {
            LogWithContext(logger, LogLevel.Debug, message, context);
        }

        public static void ErrorWithContext(this ILogger logger, string message, object context)
        {
            LogWithContext(logger, LogLevel.Error, message, context);
        }

        public static void FatalWithContext(this ILogger logger, string message, object context)
        {
            LogWithContext(logger, LogLevel.Fatal, message, context);
        }

        public static void InfoWithContext(this ILogger logger, string message, object context)
        {
            LogWithContext(logger, LogLevel.Info, message, context);
        }

        public static void TraceWithContext(this ILogger logger, string message, object context)
        {
            LogWithContext(logger, LogLevel.Trace, message, context);
        }

        public static void WarnWithContext(this ILogger logger, string message, object context)
        {
            LogWithContext(logger, LogLevel.Warn, message, context);
        }
    }
}
