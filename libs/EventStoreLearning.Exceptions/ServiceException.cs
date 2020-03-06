using System;
namespace EventStoreLearning.Exceptions
{
    public class ServiceException : EnvironmentException
    {
        private const string reasonCodePrefix = "S";

        public ServiceException(string serviceName, string reasonCode, string message) : base($"{reasonCodePrefix}{reasonCode}", message)
        {
            ServiceName = serviceName;
        }

        public ServiceException(string serviceName, string reasonCode, string message, Exception innerException) : base($"{reasonCodePrefix}{reasonCode}", message, innerException)
        {
            ServiceName = serviceName;
        }

        public string ServiceName { get; }
    }
}
