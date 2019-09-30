using System;
namespace EventStoreLearning.Common.EventSourcing
{
    public class CommandResponse<TCommand> where TCommand : Command
    {
        public CommandResponse(TCommand command, object response, Exception error)
        {
            Command = command;
            Response = response;
            Error = error;
        }

        public TCommand Command { get; }
        public object Response { get; }
        public Exception Error { get; }
    }
}
