using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventStoreLearning.Common.EventSourcing
{
    public interface IEventRepository
    {
        Task<Exception> Save<T>(T aggregate, long expectedVersion = -1) where T : AggregateRoot;
        Task<T> GetAggregateById<T>(Guid id) where T : AggregateRoot, new();
        Task<List<Event>> GetAllEventsForAggregateType<T>(long start) where T : AggregateRoot;
        Task<List<Event>> GetAllEventsOfType<T>(long start) where T : Event;
    }
}
