using System;
namespace EventStoreLearning.Common.EventSourcing
{
    public class EventMetadata
    {
        public static EventMetadata FromCausingEvent(EventMetadata eventData)
        {
            return new EventMetadata
            {
                CorrelationId = eventData.CorrelationId,
                CausationId = eventData.Id
            };
        }

        public static EventMetadata FromObject(dynamic eventData)
        {
            return new EventMetadata
            {
                SourceApplication = eventData.SourceApplication,
                Id = eventData.Id ?? new Guid(),
                CorrelationId = eventData.CorrelationId ?? new Guid(),
                CausationId = eventData.Id ?? new Guid()
            };
        }

        public EventMetadata()
        {
            SourceApplication = "EventStoreLearningAPI";

            Id = Guid.NewGuid();
            CorrelationId = Id;
            CausationId = Id;
        }


        public string SourceApplication { get; private set; }
        public Guid Id { get; private set; }
        public Guid CorrelationId { get; private set; }
        public Guid CausationId { get; private set; }
    }
}
