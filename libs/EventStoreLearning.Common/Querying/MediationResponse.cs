using System;

namespace EventStoreLearning.Common.Querying
{
    public class MediationResponse<TRequest, TResponse>
    {
        public MediationResponse(TRequest request, TResponse response)
        {
            Request = request;
            Response = response;
        }

        public TRequest Request { get; }
        public TResponse Response { get; }
    }
}
