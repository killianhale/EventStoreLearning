using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModel.Queries;
using MediatR;

namespace EventStoreLearning.Appointment.ReadModel
{
    public interface IAppointmentQueryMediator
    {
        Task<QueryResponse<TQuery, TResult>> Query<TQuery, TResult>(TQuery query, CancellationToken cancelationToken);
    }
}