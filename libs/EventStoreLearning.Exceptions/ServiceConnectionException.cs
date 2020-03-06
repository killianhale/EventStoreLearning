using System;
namespace EventStoreLearning.Exceptions
{
    public class ServiceConnectionException : ServiceException
    {
        private const string reasonCode = "CONNECT";

        public ServiceConnectionException(string serviceName)
            : base(serviceName, reasonCode, $"There was a problem with the connection to the service '{serviceName}'")
        {
        }

        public ServiceConnectionException(string serviceName, Exception innerException)
            : base(serviceName, reasonCode, $"There was a problem with the connection to the service '{serviceName}'", innerException)
        {
        }
    }
}
