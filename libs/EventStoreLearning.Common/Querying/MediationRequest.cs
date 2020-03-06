using System;
using MediatR;

namespace EventStoreLearning.Common.Querying
{
    public class MediationRequest<TRequest, TResponse> : IRequest<MediationResponse<TRequest, TResponse>>
    {
        public MediationRequest(TRequest request)
        {
            Request = request;
        }

        public TRequest Request { get; }
    }
}
