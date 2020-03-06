using System;
namespace EventStoreLearning.Common.Web.Models
{
    /// <summary>
    /// Contains information to return to the caller about an error that has occurred 
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Must provide a message when creating an error response
        /// </summary>
        /// <param name="message"></param>
        /// <param name="stackTrace"></param>
        public ErrorResponse(string message, string reasonCode = null, string stackTrace = null)
        {
            Message = message;
            ReasonCode = reasonCode;
            StackTrace = stackTrace;
        }

        /// <summary>
        /// A unique code representing the reason for the error
        /// </summary>
        public string ReasonCode { get; }

        /// <summary>
        /// A summary of the error
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// A stack trace of where the error occurred
        /// </summary>
        public string StackTrace { get; }
    }
}
