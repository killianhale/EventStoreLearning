using System.Collections.Generic;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModels.Models;

namespace EventStoreLearning.Appointment.ReadModel.Repositories
{
    public interface IAppointmentRepository
    {
        Task CreateAppointment(AppointmentReadModel model);
        Task<IList<AppointmentReadModel>> RetrieveAllAppointments();
    }
}