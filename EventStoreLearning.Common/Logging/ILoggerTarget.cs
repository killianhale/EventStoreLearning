using System;
namespace EventStoreLearning.Common.Logging
{
    public interface ILoggerTarget
    {
        void Log(string name, string type, string message, object context = null);

        void Debug(string name, string message, object context = null);
        void Error(string name, string message, object context = null);
        void Fatal(string name, string message, object context = null);
        void Info(string name, string message, object context = null);
        void Trace(string name, string message, object context = null);
        void Warn(string name, string message, object context = null);
    }
}
