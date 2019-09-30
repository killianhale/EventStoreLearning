using System;

namespace EventStoreLearning.Appointment.ReadModel.Queries
{
    public class QueryResponse<TQuery, TResponse>
    {
        public QueryResponse(TQuery query, TResponse response, Exception error)
        {
            Query = query;
            Response = response;
            Error = error;
        }

        public TQuery Query { get; }
        public TResponse Response { get; }
        public Exception Error { get; }
    }
}
