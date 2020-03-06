using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventStoreLearning.EventSourcing
{
    public interface IAggregateOrchestrator
    {
        Task<Guid> Change<T>(Guid id, long expectedVersion, Action<Dictionary<string, AggregateRoot>, T> action) where T : AggregateRoot, new();
        Task<Guid> Create<T>(Func<Dictionary<string, AggregateRoot>, T> action) where T : AggregateRoot, new();
        IAggregateOrchestrator FetchDependency<T>(Guid id) where T : AggregateRoot, new();
    }
}