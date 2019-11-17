using System;
namespace EventStoreLearning.Common.EventSourcing
{
    public interface IResponse<TRequest, TResponse>
    {
        TRequest Request { get; }
        TResponse Response { get; }
        Exception Error { get; }

    }
}
