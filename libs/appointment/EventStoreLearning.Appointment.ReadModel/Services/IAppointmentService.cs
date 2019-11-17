using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModel.Queries;
using EventStoreLearning.Appointment.ReadModels.Models;
using EventStoreLearning.Common.Querying;

namespace EventStoreLearning.Appointment.ReadModel.Services
{
    public interface IAppointmentService
    {
        Task<QueryResponse<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>>> Handle(QueryRequest<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>> request, CancellationToken cancellationToken);
    }
}