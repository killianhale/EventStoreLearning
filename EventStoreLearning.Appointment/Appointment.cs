using System;
using System.Collections.Generic;
using EventStoreLearning.Common.EventSourcing;
using EventStoreLearning.Appointment.Events;

namespace EventStoreLearning.Appointment
{
    public class Appointment : AggregateRoot
    {
        public override Guid Id => _id;
        public override long Version => _version;

        public Appointment()
        {
            _handlers = new Dictionary<Type, Action<Event>>
            {
                { typeof(AppointmentCreatedEvent), (e) => Apply(e as AppointmentCreatedEvent) }
            };
        }

        public Appointment(Guid id, string title, DateTime startTime, TimeSpan duration) : this()
        {
            ApplyChange(new AppointmentCreatedEvent(id, title, startTime, duration));
        }

        private void Apply(AppointmentCreatedEvent e)
        {
            _id = e.AggregateId;
        }
    }
}
