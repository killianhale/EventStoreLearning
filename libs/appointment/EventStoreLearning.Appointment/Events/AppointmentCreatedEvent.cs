using System;
using EventStoreLearning.Common.EventSourcing;
using Newtonsoft.Json;

namespace EventStoreLearning.Appointment.Events
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AppointmentCreatedEvent : Event
    {
        public AppointmentCreatedEvent(Guid id, string title, DateTime startTime, TimeSpan duration)
        {
            AggregateId = id;
            Title = title;
            StartTime = startTime;
            Duration = duration;
        }

        [JsonProperty]
        public string Title { get; }

        [JsonProperty]
        public DateTime StartTime { get; }

        [JsonProperty]
        public TimeSpan Duration { get; }
    }
}
