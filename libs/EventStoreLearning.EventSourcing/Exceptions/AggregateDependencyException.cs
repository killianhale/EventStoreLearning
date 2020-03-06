using System;
namespace EventStoreLearning.EventSourcing.Exceptions
{
    public interface IAggregateDependencyException
    {
        string ReasonCode { get; }
    }

    public class AggregateDependencyException<T> : AggregateRootException<T>, IAggregateDependencyException where T : AggregateRoot, new()
    {
        private const string reasonCode = "DEP";

        public AggregateDependencyException(Guid id, string message) : base(id, reasonCode, message)
        {
        }

        public AggregateDependencyException(Guid id, string message, Exception innerException) : base(id, reasonCode, message, innerException)
        {
        }
    }
}
