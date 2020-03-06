using System;
namespace EventStoreLearning.Exceptions
{
    public class ServiceAuthenticationException : ServiceException
    {
        private const string reasonCode = "AUTH";

        public ServiceAuthenticationException(string serviceName)
            : base(serviceName, reasonCode, $"There was a problem with the service account for the service '{serviceName}'")
        {
        }

        public ServiceAuthenticationException(string serviceName, Exception innerException)
            : base(serviceName, reasonCode, $"There was a problem with the service account for the service '{serviceName}'", innerException)
        {
        }
    }
}
