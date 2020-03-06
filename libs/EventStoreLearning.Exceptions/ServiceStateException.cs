using System;
namespace EventStoreLearning.Exceptions
{
    public class ServiceStateException : ServiceException
    {
        private const string reasonCode = "STATE";

        public ServiceStateException(string serviceName)
            : base(serviceName, reasonCode, $"There was a problem with the state of the service '{serviceName}'")
        {
        }

        public ServiceStateException(string serviceName, Exception innerException)
            : base(serviceName, reasonCode, $"There was a problem with the state of the service '{serviceName}'", innerException)
        {
        }
    }
}
