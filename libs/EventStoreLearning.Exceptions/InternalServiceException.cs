using System;
namespace EventStoreLearning.Exceptions
{
    public class InternalServiceException : ServiceException
    {
        private const string reasonCode = "INTERNAL";

        public InternalServiceException(string serviceName)
            : base(serviceName, reasonCode, $"There was a problem performing the desired operation within the service '{serviceName}'")
        {
        }

        public InternalServiceException(string serviceName, Exception innerException)
            : base(serviceName, reasonCode, $"There was a problem performing the desired operation within the service '{serviceName}'", innerException)
        {
        }
    }
}
