using System;
namespace EventStoreLearning.EventSourcing
{
    public class EventModel
    {
        public EventModel(IEvent e, EventMetadata metadata, long version = -1)
        {
            Event = e;
            Metadata = metadata;
            Version = version;
        }

        public IEvent Event { get; private set; }
        public EventMetadata Metadata { get; private set; }
        public long Version { get; private set; }
    }
}
