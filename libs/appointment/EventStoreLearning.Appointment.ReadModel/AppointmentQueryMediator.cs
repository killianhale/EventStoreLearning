using System;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModel.Queries;
using EventStoreLearning.Common.Querying;
using MediatR;

namespace EventStoreLearning.Appointment.ReadModel
{
    public class AppointmentQueryMediator : IQuery
    {
        private readonly IMediator _mediator;

        public AppointmentQueryMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<QueryResponse<TQuery, TResult>> Query<TQuery, TResult>(TQuery query, CancellationToken cancelationToken)
        {
            var request = new QueryRequest<TQuery, TResult>(query);

            return await _mediator.Send(request, cancelationToken);
        }
    }
}
