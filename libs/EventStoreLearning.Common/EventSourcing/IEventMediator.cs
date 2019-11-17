using System.Threading;
using System.Threading.Tasks;

namespace EventStoreLearning.Common.EventSourcing
{
    public interface IEventMediator
    {
        Task PublishEvent<TEvent>(TEvent e, CancellationToken cancelationToken) where TEvent : Event;
        Task PublishEvent(Event e, CancellationToken cancelationToken);
    }
}