using EventStore.ClientAPI;

namespace EventStoreLearning.EventStore
{
    public interface IEventStoreClient
    {
        IEventStoreConnection Connect();
    }
}