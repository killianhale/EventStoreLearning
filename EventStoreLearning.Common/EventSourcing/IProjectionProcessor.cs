namespace EventStoreLearning.Common.EventSourcing
{
    public interface IProjectionProcessor
    {
        void Start();
        void Stop();
    }
}