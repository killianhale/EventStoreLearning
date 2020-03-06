using System;
namespace EventStoreLearning.Exceptions
{
    public class AuthenticationException : ReasonCodeException
    {
        private const string reasonCodePrefix = "AUTH";

        public AuthenticationException(string message) : base(reasonCodePrefix, message)
        {
        }

        public AuthenticationException(string message, Exception innerException) : base(reasonCodePrefix, message, innerException)
        {
        }

        public AuthenticationException(string reasonCode, string message) : base($"{reasonCodePrefix}{reasonCode}", message)
        {
        }

        public AuthenticationException(string reasonCode, string message, Exception innerException) : base($"{reasonCodePrefix}{reasonCode}", message, innerException)
        {
        }
    }
}
