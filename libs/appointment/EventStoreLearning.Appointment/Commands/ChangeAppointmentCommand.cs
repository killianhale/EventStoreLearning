using System;
using AggregateOP.MediatR;

namespace EventStoreLearning.Appointment.Commands
{
    public class ChangeAppointmentCommand : IMediatedCommand
    {
        public ChangeAppointmentCommand(Guid id, long version, string title, DateTime? startTime, TimeSpan? duration)
        {
            Id = id;
            Version = version;
            Title = title;
            StartTime = startTime;
            Duration = duration;
        }

        public Guid Id { get; }
        public long Version { get; }
        public string Title { get; }
        public DateTime? StartTime { get; }
        public TimeSpan? Duration { get; }
    }
}
