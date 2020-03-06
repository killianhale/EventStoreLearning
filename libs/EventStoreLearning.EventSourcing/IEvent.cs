using System;

namespace EventStoreLearning.EventSourcing
{
    public interface IEvent : IMessage<Guid>
    {
        Guid AggregateId { get; }
    }
}
