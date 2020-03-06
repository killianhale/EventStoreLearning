using System;
using System.Threading;
using ContextRunner;
using EventStoreLearning.EventSourcing;
using EventStoreLearning.EventSourcing.EventStore;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventStoreLearning.Mediation.EventSourcing.EventStore
{
    public abstract class MediatedProjectionProcessor<T> : ProjectionProcessor<T> where T : AggregateRoot
    {
        private readonly IMediator _mediator;

        protected MediatedProjectionProcessor(ILogger<ProjectionProcessor<T>> logger, IEventRepository repo, IMediator mediator) : this(null, logger, repo, mediator) { }

        protected MediatedProjectionProcessor(IContextRunner runner, ILogger<ProjectionProcessor<T>> logger, IEventRepository repo, IMediator mediator) : base(runner, logger, repo)
        {
            _mediator = mediator;

            EventProjected += MediatedProjectionProcessor_EventProjected;
        }

        private void MediatedProjectionProcessor_EventProjected(IEvent e, CancellationToken cancelationToken)
        {
            _mediator.Send(e, cancelationToken);
        }
    }
}
