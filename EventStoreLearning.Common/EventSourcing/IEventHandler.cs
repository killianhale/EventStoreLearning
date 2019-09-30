using System;
using MediatR;

namespace EventStoreLearning.Common.EventSourcing
{
    public interface IEventHandler<TEvent> : IRequestHandler<EventRequest<TEvent>>
        where TEvent : Event
    {
    }
}
