using System;
using MediatR;

namespace EventStoreLearning.Common.EventSourcing
{
    public class EventRequest<TEvent> : IRequest
        where TEvent : Event
    {
        public EventRequest(TEvent e)
        {
            Event = e;
        }

        public TEvent Event { get; }
    }
}
