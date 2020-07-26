using System;
using AggregateOP.MediatR;
using Newtonsoft.Json;

namespace EventStoreLearning.Appointment.Events
{
    public class AppointmentCancelledEvent : IMediatedEvent
    {
        public AppointmentCancelledEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        [JsonProperty]
        public Guid AggregateId { get; }
    }
}
