using System;
using EventStoreLearning.EventSourcing;
using MediatR;

namespace EventStoreLearning.EventSourcing
{
    public interface IEventHandler<TEvent> : IRequestHandler<TEvent, Guid>
        where TEvent : IEvent
    {
    }
}
