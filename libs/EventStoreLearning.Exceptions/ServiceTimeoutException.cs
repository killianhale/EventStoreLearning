using System;
namespace EventStoreLearning.Exceptions
{
    public class ServiceTimeoutException : ServiceException
    {
        private const string reasonCode = "TO";

        public ServiceTimeoutException(string serviceName)
            : base(serviceName, reasonCode, $"A timeout has occurred with the connection to the service '{serviceName}'")
        {
        }

        public ServiceTimeoutException(string serviceName, Exception innerException)
            : base(serviceName, reasonCode, $"A timeout has occurred with the connection to the service '{serviceName}'", innerException)
        {
        }
    }
}
