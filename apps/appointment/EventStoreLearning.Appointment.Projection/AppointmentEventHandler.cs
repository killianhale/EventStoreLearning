using System;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.Events;
using EventStoreLearning.Appointment.ReadModel.Repositories;
using EventStoreLearning.Appointment.ReadModels.Models;
using MediatR;
using ContextRunner;
using AggregateOP.MediatR;

namespace EventStoreLearning.Appointment.Projection
{
    public class AppointmentEventHandler :
        IEventHandler<AppointmentCreatedEvent>,
        IEventHandler<AppointmentChangedEvent>,
        IEventHandler<AppointmentCancelledEvent>
    {
        private readonly IContextRunner _runner;
        private readonly IAppointmentRepository _appointmentRepo;

        public AppointmentEventHandler(IContextRunner runner, IAppointmentRepository appointmentRepo)
        {
            _runner = runner;
            _appointmentRepo = appointmentRepo;
        }

        public async Task<Unit> Handle(MediatedEventModel<AppointmentCreatedEvent> @event, CancellationToken cancellationToken)
        {
            await _runner.RunAction(async context =>
            {
                context.Logger.Information($"Handling event {nameof(AppointmentCreatedEvent)} for Aggregate {nameof(Appointment)}");

                var model = new AppointmentReadModel()
                {
                    Title = @event.Event.Title,
                    StartTime = @event.Event.StartTime,
                    Duration = (int)@event.Event.Duration.TotalMinutes,
                    Id = @event.Event.AggregateId,
                    Version = @event.Version
                };

                await _appointmentRepo.CreateAppointment(model);
            });

            //return @event.Event.AggregateId;
            return new Unit();
        }

        public async Task<Unit> Handle(MediatedEventModel<AppointmentChangedEvent> model, CancellationToken cancellationToken)
        {
            await _runner.RunAction(async context =>
            {
                context.Logger.Information($"Handling event {nameof(AppointmentChangedEvent)} for Aggregate {nameof(Appointment)}");

                await _appointmentRepo.UpdateAppointment(
                    model.Event.AggregateId,
                    model.Version,
                    model.Event.Title,
                    model.Event.StartTime,
                    model.Event.Duration);
            });

            //return model.Event.AggregateId;
            return new Unit();
        }

        public async Task<Unit> Handle(MediatedEventModel<AppointmentCancelledEvent> model, CancellationToken cancellationToken)
        {
            await _runner.RunAction(async context =>
            {
                context.Logger.Information($"Handling event {nameof(AppointmentCancelledEvent)} for Aggregate {nameof(Appointment)}");

                await _appointmentRepo.DeleteAppointment(model.Event.AggregateId);
            });

            //return model.Event.AggregateId;
            return new Unit();
        }
    }
}
