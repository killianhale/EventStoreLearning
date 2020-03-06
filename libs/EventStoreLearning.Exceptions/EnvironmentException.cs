using System;
namespace EventStoreLearning.Exceptions
{
    public class EnvironmentException : ReasonCodeException
    {
        private const string reasonCodePrefix = "EN";

        public EnvironmentException(string message) : base(reasonCodePrefix, message)
        {
        }

        public EnvironmentException(string message, Exception innerException) : base(reasonCodePrefix, message, innerException)
        {
        }

        public EnvironmentException(string reasonCode, string message) : base($"{reasonCodePrefix}{reasonCode}", message)
        {
        }

        public EnvironmentException(string reasonCode, string message, Exception innerException) : base($"{reasonCodePrefix}{reasonCode}", message, innerException)
        {
        }
    }
}
