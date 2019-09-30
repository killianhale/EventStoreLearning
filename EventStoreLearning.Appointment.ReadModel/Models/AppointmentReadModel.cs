using System;
namespace EventStoreLearning.Appointment.ReadModels.Models
{
    public class AppointmentReadModel
    {
        public string Title;
        public DateTime StartTime;
        public string Duration;
        public Guid Id;
    }
}
