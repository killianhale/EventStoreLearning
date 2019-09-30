using System.Threading;
using System.Threading.Tasks;

namespace EventStoreLearning.Common.EventSourcing
{
    public interface ICommandMediator
    {
        Task<CommandResponse<TCommand>> PublishCommand<TCommand>(TCommand command, CancellationToken cancelationToken)
            where TCommand : Command;
    }
}