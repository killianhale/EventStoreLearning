using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.Commands;
using EventStoreLearning.Common.EventSourcing;
using EventStoreLearning.Common.Logging;
using MediatR;
using NLog;

namespace EventStoreLearning.Appointment
{
    public class AppointmentCommandHandler : ICommandHandler<CreateAppointmentCommand>
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IEventRepository _repo;

        public AppointmentCommandHandler(IEventRepository repo)
        {
            _repo = repo;
        }

        public async Task<CommandResponse<CreateAppointmentCommand>> Handle(CommandRequest<CreateAppointmentCommand> request, CancellationToken cancellationToken)
        {
            var command = request.Command;

            _logger.DebugWithContext($"Handling command {nameof(CreateAppointmentCommand)} for Aggregate {nameof(Appointment)}", command);

            var appointment = new Appointment(command.Id, command.Title, command.StartTime, command.Duration);

            var error = await _repo.Save(appointment);

            return new CommandResponse<CreateAppointmentCommand>(command, null, error);
        }

    }
}
