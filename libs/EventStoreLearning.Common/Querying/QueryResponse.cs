using System;
using EventStoreLearning.Common.EventSourcing;

namespace EventStoreLearning.Common.Querying
{
    public class QueryResponse<TQuery, TResponse> : IResponse<TQuery, TResponse>
    {
        public QueryResponse(TQuery query, TResponse response, Exception error)
        {
            Request = query;
            Response = response;
            Error = error;
        }

        public TQuery Request { get; }
        public TResponse Response { get; }
        public Exception Error { get; }
    }
}
