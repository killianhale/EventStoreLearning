using System;
using MediatR;

namespace EventStoreLearning.EventSourcing.Commands
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Guid>
        where TCommand : ICommand
    {
    }
}
