using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModel.Queries;
using EventStoreLearning.Appointment.ReadModel.Repositories;
using EventStoreLearning.Appointment.ReadModels.Models;
using EventStoreLearning.Common.Querying;
using ContextRunner;
using MediatR;

namespace EventStoreLearning.Appointment.ReadModel.Services
{
    public class AppointmentService : IRequestHandler<MediationRequest<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>>, MediationResponse<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>>>, IAppointmentService
    {
        private readonly IContextRunner _runner;
        private readonly IAppointmentRepository _repo;

        public AppointmentService(IContextRunner runner, IAppointmentRepository repo)
        {
            _runner = runner;
            _repo = repo;
        }

        public async Task<MediationResponse<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>>> Handle(MediationRequest<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>> request, CancellationToken cancellationToken)
        {
            return await _runner.RunAction(async context =>
            {
                var list = await _repo.RetrieveAllAppointments();

                return new MediationResponse<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>>(request.Request, list);
            }, nameof(AppointmentService));
        }
    }
}
