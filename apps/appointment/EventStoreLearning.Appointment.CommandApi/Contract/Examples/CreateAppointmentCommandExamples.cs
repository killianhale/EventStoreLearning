using System;
using EventStoreLearning.Appointment.Commands;
using EventStoreLearning.Common.Web.Models;
using Swashbuckle.AspNetCore.Filters;

namespace EventStoreLearning.Appointment.CommandApi.Contract.Examples
{
    internal class CreateAppointmentCommandExamples : IExamplesProvider<CreateAppointmentRequest>, IExamplesProvider<CreateAppointmentCommand>, IExamplesProvider<ErrorResponse>
    {
        CreateAppointmentRequest IExamplesProvider<CreateAppointmentRequest>.GetExamples()
        {
            return new CreateAppointmentRequest
            {
                Title = "Sample Meeting #1",
                StartTime = DateTime.UtcNow,
                DurationMinutes = 30
            };
        }

        CreateAppointmentCommand IExamplesProvider<CreateAppointmentCommand>.GetExamples()
        {
            return new CreateAppointmentCommand(
                "Sample Meeting #1",
                DateTime.UtcNow,
                TimeSpan.FromMinutes(30)
                );
        }

        ErrorResponse IExamplesProvider<ErrorResponse>.GetExamples()
        {
            return new ErrorResponse("There was a problem while trying to create the appointment");
        }
    }
}
