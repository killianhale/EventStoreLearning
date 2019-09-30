using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventStoreLearning.Common.EventSourcing
{
    public class EventMediator : IEventMediator
    {
        private readonly IMediator _mediator;

        public EventMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishEvent<TEvent>(TEvent e, CancellationToken cancelationToken)
            where TEvent : Event
        {
            var request = new EventRequest<TEvent>(e);

            await _mediator.Send(request, cancelationToken);
        }

        public async Task PublishEvent(Event e, CancellationToken cancelationToken)
        {
            var requestType = typeof(EventRequest<>);
            requestType = requestType.MakeGenericType(e.GetType());

            var request = Activator.CreateInstance(requestType, e);

            var send = typeof(IMediator).GetMethod("Send", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
            send = send.MakeGenericMethod(typeof(Unit));

            await (dynamic)send.Invoke(_mediator, new[] { request, cancelationToken });
        }
    }
}
