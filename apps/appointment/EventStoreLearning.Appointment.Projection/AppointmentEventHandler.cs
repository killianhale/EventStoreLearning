using System;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.Events;
using EventStoreLearning.Appointment.ReadModel.Repositories;
using EventStoreLearning.Appointment.ReadModels.Models;
using EventStoreLearning.EventSourcing;
using MediatR;
using ContextRunner;

namespace EventStoreLearning.Appointment.Projection
{
    public class AppointmentEventHandler : IEventHandler<AppointmentCreatedEvent>
    {
        private readonly IContextRunner _runner;
        private readonly IAppointmentRepository _appointmentRepo;

        public AppointmentEventHandler(IContextRunner runner, IAppointmentRepository appointmentRepo)
        {
            _runner = runner;
            _appointmentRepo = appointmentRepo;
        }

        public async Task<Guid> Handle(AppointmentCreatedEvent @event, CancellationToken cancellationToken)
        {
            await _runner.RunAction(async context =>
            {
                context.State.SetParam("eventType", nameof(AppointmentCreatedEvent));
                context.State.SetParam("event", @event);

                context.Logger.Information($"Handling event {nameof(AppointmentCreatedEvent)} for Aggregate {nameof(Appointment)}");

                var model = new AppointmentReadModel()
                {
                    Title = @event.Title,
                    StartTime = @event.StartTime,
                    Duration = @event.Duration.ToString(),
                    Id = @event.AggregateId
                };

                await _appointmentRepo.CreateAppointment(model);
            });

            return @event.AggregateId;
        }
    }
}
