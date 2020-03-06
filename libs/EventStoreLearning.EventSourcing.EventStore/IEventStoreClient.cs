using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using ContextRunner.Base;

namespace EventStoreLearning.EventSourcing.EventStore
{
    public interface IEventStoreClient
    {
        Task ConnectWithContext(Func<IEventStoreConnection, ActionContext, Task> action, string contextSubName = null);
    }
}