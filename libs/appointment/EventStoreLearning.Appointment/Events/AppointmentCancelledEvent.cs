using System;
using EventStoreLearning.EventSourcing;
using Newtonsoft.Json;

namespace EventStoreLearning.Appointment.Events
{
    public class AppointmentCancelledEvent : IEvent
    {
        public AppointmentCancelledEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        [JsonProperty]
        public Guid AggregateId { get; }
    }
}
