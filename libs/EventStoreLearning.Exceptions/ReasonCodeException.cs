using System;
namespace EventStoreLearning.Exceptions
{
    public class ReasonCodeException : Exception
    {
        public ReasonCodeException(string reasonCode, string message) : this(reasonCode, message, null) {}

        public ReasonCodeException(string reasonCode, string message, Exception innerException) : base(message, innerException)
        {
            ReasonCode = reasonCode;
        }

        public string ReasonCode { get; protected set; }
    }
}
