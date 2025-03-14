
namespace Vitese_Payment_Processing.Logging
{
    public interface ILogger
    {
        bool IsDebugEnabled { get; }
        bool IsVerboseEnabled { get; }
        bool IsInformationalEnabled { get; }
        bool IsWarningEnabled { get; }
        bool IsFatalEnabled { get; }
        bool IsErrorEnabled { get; }

        void Write(string message, EventSeverity severity);
        void Write(string message, Exception exception, EventSeverity severity);
    }
}
