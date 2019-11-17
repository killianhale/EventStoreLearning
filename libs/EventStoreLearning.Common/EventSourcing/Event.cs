using System;
using Newtonsoft.Json;

namespace EventStoreLearning.Common.EventSourcing
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Event : IMessage
    {
        public Event()
        {
            metadata = new EventMetadata();
        }

        [JsonProperty]
        public Guid AggregateId { get; protected set; }

        [JsonIgnore]
        private long version;
        public long GetVersion() => version;

        public void SetVersion(long value)
        {
            version = value;
        }

        [JsonIgnore]
        private EventMetadata metadata;
        public EventMetadata GetMetadata() => metadata;

        public void SetMetadata(EventMetadata value)
        {
            metadata = value;
        }
    }
}
