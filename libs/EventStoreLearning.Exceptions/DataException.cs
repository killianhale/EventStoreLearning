using System;
namespace EventStoreLearning.Exceptions
{
    public class DataException : ReasonCodeException
    {
        private const string reasonCodePrefix = "D";

        public DataException(string message) : base(reasonCodePrefix, message)
        {
        }

        public DataException(string message, Exception innerException) : base(reasonCodePrefix, message, innerException)
        {
        }

        public DataException(string reasonCode, string message) : base($"{reasonCodePrefix}{reasonCode}", message)
        {
        }

        public DataException(string reasonCode, string message, Exception innerException) : base($"{reasonCodePrefix}{reasonCode}", message, innerException)
        {
        }
    }
}
