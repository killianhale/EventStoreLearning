using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModel.Queries;
using EventStoreLearning.Appointment.ReadModel.Repositories;
using EventStoreLearning.Appointment.ReadModels.Models;
using MediatR;

namespace EventStoreLearning.Appointment.ReadModel.Services
{
    public class AppointmentService : IRequestHandler<QueryRequest<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>>, QueryResponse<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>>>, IAppointmentService
    {
        private readonly IAppointmentRepository _repo;

        public AppointmentService(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<QueryResponse<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>>> Handle(QueryRequest<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>> request, CancellationToken cancellationToken)
        {
            var list = await _repo.RetrieveAllAppointments();

            return new QueryResponse<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>>(request.Query, list, null);
        }
    }
}
