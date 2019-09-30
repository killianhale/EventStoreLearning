using System;
using EventStoreLearning.Common.EventSourcing;

namespace EventStoreLearning.Appointment.Events
{
    public class AppointmentCreatedEvent : Event
    {
        public AppointmentCreatedEvent(Guid id, string title, DateTime startTime, TimeSpan duration)
        {
            AggregateId = id;
            Title = title;
            StartTime = startTime;
            Duration = duration;
        }

        public string Title { get; }
        public DateTime StartTime { get; }
        public TimeSpan Duration { get; }
    }
}
