using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModels.Models;

namespace EventStoreLearning.Appointment.ReadModel.Repositories
{
    public interface IAppointmentRepository
    {
        Task CreateAppointment(AppointmentReadModel model);
        Task ReplaceAppointment(AppointmentReadModel model);
        Task UpdateAppointment(Guid id, long version, string title, DateTime? startTime, TimeSpan? duration);
        Task DeleteAppointment(Guid id);

        Task<IList<AppointmentReadModel>> RetrieveAllAppointments();
    }
}