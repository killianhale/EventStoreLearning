using System;
using System.Collections.Generic;
using EventStoreLearning.EventSourcing;
using EventStoreLearning.Appointment.Events;

namespace EventStoreLearning.Appointment
{
    public class Appointment : AggregateRoot
    {
        public override Guid Id => _id;

        private string _title;
        private DateTime _startTime;

        public Appointment() : base("APNT")
        {
            _handlers = new Dictionary<Type, Action<IEvent>>
            {
                { typeof(AppointmentCreatedEvent), (e) => Apply(e as AppointmentCreatedEvent) },
                { typeof(AppointmentChangedEvent), (e) => Apply(e as AppointmentChangedEvent) }
            };
        }

        public Appointment(string title, DateTime startTime, TimeSpan duration) : this()
        {
            Validate(title, startTime);

            ApplyChange(new AppointmentCreatedEvent(Guid.NewGuid(), title, startTime, duration));
        }

        public void Update(string title, DateTime? startTime, TimeSpan? duration)
        {
            Validate(title, startTime);

            ApplyChange(new AppointmentChangedEvent(_id, title, startTime, duration));
        }

        public void Cancel()
        {
            ApplyChange(new AppointmentCancelledEvent(_id));
        }

        private void Validate(string title, DateTime? startTime)
        {
            var t = title ?? _title;
            var s = startTime ?? _startTime;

            if (string.IsNullOrWhiteSpace(t))
            {
                throw new ArgumentException("The appointment must have a title!");
            }

            if (s == null)
            {
                throw new ArgumentException("The appointment must have a start time!");
            }
        }

        private void Apply(AppointmentCreatedEvent e)
        {
            _id = e.AggregateId;
            _title = e.Title;
            _startTime = e.StartTime;
        }

        private void Apply(AppointmentChangedEvent e)
        {
            _id = e.AggregateId;
            _title = e.Title;
            _startTime = e.StartTime ?? DateTime.MinValue;
        }
    }
}
