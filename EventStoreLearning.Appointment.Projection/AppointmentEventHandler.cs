using System;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.Events;
using EventStoreLearning.Appointment.ReadModel.Repositories;
using EventStoreLearning.Appointment.ReadModels.Models;
using EventStoreLearning.Common.EventSourcing;
using EventStoreLearning.Common.Logging;
using MediatR;
using NLog;

namespace EventStoreLearning.Appointment.Projection
{
    public class AppointmentEventHandler : IEventHandler<AppointmentCreatedEvent>
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IAppointmentRepository _appointmentRepo;

        public AppointmentEventHandler(IAppointmentRepository appointmentRepo)
        {
            _appointmentRepo = appointmentRepo;
        }

        public async Task<Unit> Handle(EventRequest<AppointmentCreatedEvent> request, CancellationToken cancellationToken)
        {
            _logger.DebugWithContext($"Handling event {nameof(AppointmentCreatedEvent)} for Aggregate {nameof(Appointment)}", request.Event);

            var model = new AppointmentReadModel()
            {
                Title = request.Event.Title,
                StartTime = request.Event.StartTime,
                Duration = request.Event.Duration.ToString(),
                Id = request.Event.AggregateId
            };

            await _appointmentRepo.CreateAppointment(model);

            return new Unit();
        }
    }
}
