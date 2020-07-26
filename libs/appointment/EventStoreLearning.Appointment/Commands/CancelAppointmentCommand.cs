using System;
using AggregateOP.MediatR;

namespace EventStoreLearning.Appointment.Commands
{
    public class CancelAppointmentCommand : IMediatedCommand
    {
        public CancelAppointmentCommand(Guid id, long version)
        {
            Id = id;
            Version = version;
        }

        public Guid Id { get; }
        public long Version { get; }
    }
}
