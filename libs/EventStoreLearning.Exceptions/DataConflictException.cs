using System;
namespace EventStoreLearning.Exceptions
{
    public class DataConflictException : DataException
    {
        private const string reasonCode = "CONFLICT";

        public DataConflictException(string message) : base(reasonCode, message)
        {
        }

        public DataConflictException(string message, Exception innerException) : base(reasonCode, message, innerException)
        {
        }
    }
}
