using System;
using EventStoreLearning.EventSourcing.Commands;

namespace EventStoreLearning.Appointment.Commands
{
    public class CancelAppointmentCommand : ICommand
    {
        public CancelAppointmentCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
