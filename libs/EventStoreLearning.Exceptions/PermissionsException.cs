using System;
namespace EventStoreLearning.Exceptions
{
    public class PermissionsException : AuthenticationException
    {
        private const string reasonCode = "P";

        public PermissionsException(string message) : base(reasonCode, message)
        {
        }

        public PermissionsException(string message, Exception innerException) : base(reasonCode, message, innerException)
        {
        }
    }
}
