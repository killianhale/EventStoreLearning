using System;
using EventStoreLearning.Common.EventSourcing;

namespace EventStoreLearning.Appointment.Events
{
    public class CreateAppointmentCommand : Command
    {
        public CreateAppointmentCommand(Guid id, string title, DateTime startTime, TimeSpan duration)
        {
            Id = id;
            Title = title;
            StartTime = startTime;
            Duration = duration;
        }

        public Guid Id { get; }
        public string Title { get; }
        public DateTime StartTime { get; }
        public TimeSpan Duration { get; }
    }
}
