using System;
using AggregateOP.MediatR;
using Newtonsoft.Json;

namespace EventStoreLearning.Appointment.Events
{
    public class AppointmentCreatedEvent : IMediatedEvent
    {
        public AppointmentCreatedEvent(Guid aggregateId, string title, DateTime startTime, TimeSpan duration)
        {
            AggregateId = aggregateId;
            Title = title;
            StartTime = startTime;
            Duration = duration;
        }

        [JsonProperty]
        public Guid AggregateId { get; }

        [JsonProperty]
        public string Title { get; }

        [JsonProperty]
        public DateTime StartTime { get; }

        [JsonProperty]
        public TimeSpan Duration { get; }
    }
}
