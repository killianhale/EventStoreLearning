using System;
using System.Collections.Generic;
using EventStoreLearning.Common.Logging;
using NLog;

namespace EventStoreLearning.Common.EventSourcing
{
    public abstract class AggregateRoot
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly List<Event> _changes;

        protected IDictionary<Type, Action<Event>> _handlers;
        protected Guid _id;
        protected long _version;

        public abstract Guid Id { get; }
        public abstract long Version { get; }

        protected AggregateRoot()
        {
            _version = -1;

            _changes = new List<Event>();
            _handlers = new Dictionary<Type, Action<Event>>();
        }

        public IEnumerable<Event> GetUncommittedChanges()
        {
            _logger.DebugWithContext($"Fetching uncommited changes for {FriendlyName}", _changes);

            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _logger.Debug($"Marking changes as committed. Clearing uncommitted list for {FriendlyName}");

            _changes.Clear();
        }

        public void LoadFromHistory(IEnumerable<Event> history)
        {
            _logger.Debug($"Loading {FriendlyName} from event history");

            foreach (var e in history)
            {
                _logger.TraceWithContext($"{FriendlyName} - Applying event from history", e);

                ApplyChange(e, false);
            }
        }

        protected void ApplyChange(Event e)
        {
            ApplyChange(e, true);
        }

        private void ApplyChange(Event e, bool isNew)
        {
            _logger.InfoWithContext($"Applying event of type {e.GetType().Name} to {FriendlyName}", e);

            _version = e.GetVersion();

            var type = e.GetType();

            if (_handlers.ContainsKey(type))
            {
                _handlers[type](e);
            }
            else
            {
                _logger.Warn($"Unable to apply change! No handler for event of type {type.Name} in {FriendlyName}");
            }

            if(isNew)
            {
                _logger.DebugWithContext($"Appending new event to uncommited changes for {FriendlyName}", e);

                _changes.Add(e);
            }
        }

        protected string FriendlyName => $"Aggragate {GetType().Name} ({Id})";
    }
}
