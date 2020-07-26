using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModel.Queries;
using EventStoreLearning.Appointment.ReadModel.Repositories;
using EventStoreLearning.Appointment.ReadModels.Models;
using ContextRunner;
using MediatR;

namespace EventStoreLearning.Appointment.ReadModel.Services
{
    public class AppointmentService : IRequestHandler<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>>, IAppointmentService
    {
        private readonly IContextRunner _runner;
        private readonly IAppointmentRepository _repo;

        public AppointmentService(IContextRunner runner, IAppointmentRepository repo)
        {
            _runner = runner;
            _repo = repo;
        }

        public async Task<IList<AppointmentReadModel>> Handle(RetrieveAllAppointmentsQuery request, CancellationToken cancellationToken)
        {
            return await _runner.RunAction(async context =>
            {
                var list = await _repo.RetrieveAllAppointments();

                return list;
            }, nameof(AppointmentService));
        }
    }
}
