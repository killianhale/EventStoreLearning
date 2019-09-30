using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventStoreLearning.Common.EventSourcing
{
    public interface IAggregateStore
    {
        Task<Exception> Save<T>(T aggregate, bool isNew, long expectedVersion = -1) where T : AggregateRoot;
        Task<T> GetById<T>(Guid id) where T : AggregateRoot;
        Task<List<Event>> GetAllEvents<T>(long start) where T : AggregateRoot;
    }
}
