using System;
using MediatR;

namespace EventStoreLearning.Common.EventSourcing
{
    public interface ICommandHandler<TCommand> :
        IRequestHandler<CommandRequest<TCommand>, CommandResponse<TCommand>>
        where TCommand : Command
    {
    }
}
