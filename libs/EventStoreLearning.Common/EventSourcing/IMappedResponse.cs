using System;
namespace EventStoreLearning.Common.EventSourcing
{
    public interface IMappedResponse<TRequest, TResponse, TMappedResponse>
    {
        TRequest Request { get; }
        TResponse Response { get; }
        TMappedResponse MappedResponse { get; }
        Exception Error { get; }

    }
}
