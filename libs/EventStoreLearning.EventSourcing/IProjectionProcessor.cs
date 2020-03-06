using System.Threading;

namespace EventStoreLearning.EventSourcing
{
    public delegate void EventProjectedHandler(IEvent e, CancellationToken cancelationToken);

    public interface IProjectionProcessor
    {
        event EventProjectedHandler EventProjected;

        void Start();
        void Stop();
    }
}