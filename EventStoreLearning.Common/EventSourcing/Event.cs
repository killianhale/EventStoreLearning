using System;
namespace EventStoreLearning.Common.EventSourcing
{
    public class Event : IMessage
    {
        public Event()
        {
            metadata = new EventMetadata();
        }

        public Guid AggregateId { get; protected set; }

        private long version;
        public long GetVersion() => version;

        public void SetVersion(long value)
        {
            version = value;
        }

        private EventMetadata metadata;
        public EventMetadata GetMetadata() => metadata;

        public void SetMetadata(EventMetadata value)
        {
            metadata = value;
        }
    }
}
