using System.Reflection;
using log4net;
using log4net.Core;

namespace Vitese_Payment_Processing.Logging
{
    public class Log4NetAdapter : ILogger
    {
        private readonly ILog _internalLogger;

        public bool IsDebugEnabled => _internalLogger.IsDebugEnabled;
        public bool IsVerboseEnabled => _internalLogger.Logger.IsEnabledFor(Level.Verbose);
        public bool IsInformationalEnabled => _internalLogger.IsInfoEnabled;
        public bool IsWarningEnabled => _internalLogger.IsWarnEnabled;
        public bool IsFatalEnabled => _internalLogger.IsFatalEnabled;
        public bool IsErrorEnabled => _internalLogger.IsErrorEnabled;

        public Log4NetAdapter(ILog internalLogger)
        {
            _internalLogger = internalLogger ?? throw new ArgumentException(nameof(internalLogger));
        }

        public void Write(string message, EventSeverity severity)
        {
            Write(message, null, severity);
        }

        public void Write(string message, Exception exception, EventSeverity severity)
        {
            var level = GetLevelFromSeverity(severity);
            _internalLogger.Logger.Log(MethodBase.GetCurrentMethod()?.DeclaringType, level, message, exception);
        }

        private static Level GetLevelFromSeverity(EventSeverity severity)
        {
            switch (severity)
            {
                case EventSeverity.Informational:
                    return Level.Info;
                case EventSeverity.Warning:
                    return Level.Warn;
                case EventSeverity.Debug:
                    return Level.Debug;
                case EventSeverity.Error:
                    return Level.Error;
                case EventSeverity.Fatal:
                    return Level.Fatal;
                case EventSeverity.Verbose:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
            }
            return Level.Info;
        }
    }
}
