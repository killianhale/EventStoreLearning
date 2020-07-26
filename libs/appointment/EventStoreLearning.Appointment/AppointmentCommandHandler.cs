using System;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.Commands;
using ContextRunner;
using AggregateOP.MediatR;
using AggregateOP;

namespace EventStoreLearning.Appointment
{
    public class AppointmentCommandHandler :
        ICommandHandler<CreateAppointmentCommand>,
        ICommandHandler<ChangeAppointmentCommand>,
        ICommandHandler<CancelAppointmentCommand>
    {
        private readonly IContextRunner _runner;
        private readonly IAggregateOrchestrator _aggregateOrchestrator;

        public AppointmentCommandHandler(IContextRunner runner, IAggregateOrchestrator aggregateOrchestrator)
        {
            _runner = runner;
            _aggregateOrchestrator = aggregateOrchestrator;
        }

        public async Task<Guid> Handle(CreateAppointmentCommand command, CancellationToken cancellationToken)
        {
            return await _runner.RunAction(async context =>
            {
                context.State.SetParam("commandType", nameof(CreateAppointmentCommand));
                context.State.SetParam("command", command);

                context.Logger.Debug($"Handling command {nameof(CreateAppointmentCommand)} for Aggregate {nameof(Appointment)}");

                return await _aggregateOrchestrator
                    //.FetchDependency<Appointment>(Guid.NewGuid())
                    .Create(deps =>
                    {
                        return new Appointment(command.Title, command.StartTime, command.Duration);
                    });

            }, nameof(AppointmentCommandHandler));
        }

        public async Task<Guid> Handle(ChangeAppointmentCommand command, CancellationToken cancellationToken)
        {
            return await _runner.RunAction(async context =>
            {
                context.State.SetParam("commandType", nameof(ChangeAppointmentCommand));
                context.State.SetParam("command", command);

                context.Logger.Debug($"Handling command {nameof(ChangeAppointmentCommand)} for Aggregate {nameof(Appointment)}");

                return await _aggregateOrchestrator
                    .Change<Appointment>(command.Id, command.Version, (deps, aggregate) =>
                    {
                        aggregate.Update(command.Title, command.StartTime, command.Duration);
                    });

            }, nameof(ChangeAppointmentCommand));
        }

        public async Task<Guid> Handle(CancelAppointmentCommand command, CancellationToken cancellationToken)
        {
            return await _runner.RunAction(async context =>
            {
                context.State.SetParam("commandType", nameof(CancelAppointmentCommand));
                context.State.SetParam("command", command);

                context.Logger.Debug($"Handling command {nameof(CancelAppointmentCommand)} for Aggregate {nameof(Appointment)}");

                return await _aggregateOrchestrator
                    .Change<Appointment>(command.Id, command.Version, (deps, aggregate) =>
                    {
                        aggregate.Cancel();
                    });

            }, nameof(CancelAppointmentCommand));
        }
    }
}
