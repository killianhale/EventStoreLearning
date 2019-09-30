using System;
using MediatR;

namespace EventStoreLearning.Appointment.ReadModel.Queries
{
    public class QueryRequest<TQuery, TResponse> : IRequest<QueryResponse<TQuery, TResponse>>
    {
        public QueryRequest(TQuery query)
        {
            Query = query;
        }

        public TQuery Query { get; }
    }
}
