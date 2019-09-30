using System;
using MediatR;

namespace EventStoreLearning.Common.EventSourcing
{
    public class CommandRequest<TCommand> : IRequest<CommandResponse<TCommand>>
        where TCommand : Command
    {
        public CommandRequest(TCommand command)
        {
            Command = command;
        }

        public TCommand Command { get; }
    }
}
