using System;
namespace EventStoreLearning.EventSourcing.Commands
{
    public interface ICommand : IMessage<Guid>
    {
    }
}
