using System;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Common.Querying;
using MediatR;

namespace EventStoreLearning.Appointment.ReadModel
{
    public class Mediator : IMediate
    {
        private readonly IMediator _mediator;

        public Mediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<MediationResponse<TQuery, TResult>> Mediate<TQuery, TResult>(TQuery query, CancellationToken cancelationToken)
        {
            var request = new MediationRequest<TQuery, TResult>(query);

            return await _mediator.Send(request, cancelationToken);
        }
    }
}
