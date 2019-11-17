using System;
namespace EventStoreLearning.Common.EventSourcing
{
    public class CommandResponse<TCommand> : IResponse<TCommand, object> where TCommand : Command
    {
        public CommandResponse(TCommand command, object response, Exception error)
        {
            Request = command;
            Response = response;
            Error = error;
        }

        public TCommand Request { get; }
        public object Response { get; }
        public Exception Error { get; }
    }
}
