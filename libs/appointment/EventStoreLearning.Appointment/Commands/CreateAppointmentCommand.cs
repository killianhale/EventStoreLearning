using System;
using EventStoreLearning.EventSourcing.Commands;

namespace EventStoreLearning.Appointment.Commands
{
    public class CreateAppointmentCommand : ICommand
    {
        public CreateAppointmentCommand(string title, DateTime startTime, TimeSpan duration)
        {
            Title = title;
            StartTime = startTime;
            Duration = duration;
        }

        public string Title { get; }
        public DateTime StartTime { get; }
        public TimeSpan Duration { get; }
    }
}
