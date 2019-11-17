using System;
using MediatR;

namespace EventStoreLearning.Common.Querying
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
