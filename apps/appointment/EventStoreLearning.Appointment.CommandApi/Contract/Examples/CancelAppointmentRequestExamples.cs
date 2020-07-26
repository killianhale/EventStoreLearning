using System;
using EventStoreLearning.Common.Web.Models;
using Swashbuckle.AspNetCore.Filters;

namespace EventStoreLearning.Appointment.CommandApi.Contract.Examples
{
    public class CancelAppointmentRequestExamples : IExamplesProvider<CancelAppointmentRequest>
    {
        public CancelAppointmentRequest GetExamples()
        {
            return new CancelAppointmentRequest
            {
                Id = Guid.NewGuid(),
                Version = 2
            };
        }
    }
}
