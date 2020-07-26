using System;
using EventStoreLearning.Common.Web.Models;
using Swashbuckle.AspNetCore.Filters;

namespace EventStoreLearning.Appointment.CommandApi.Contract.Examples
{
    public class ChangeAppointmentRequestExamples : IExamplesProvider<ChangeAppointmentRequest>
    {
        public ChangeAppointmentRequest GetExamples()
        {
            return new ChangeAppointmentRequest
            {
                Id = Guid.NewGuid(),
                Version = 2,
                Title = "Sample Meeting #1",
                StartTime = DateTime.UtcNow,
                DurationMinutes = 30
            };
        }
    }
}
