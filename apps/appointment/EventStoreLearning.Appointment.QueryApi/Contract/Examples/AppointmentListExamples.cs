using System;
using System.Collections.Generic;
using EventStoreLearning.Common.Web.Models;
using Swashbuckle.AspNetCore.Filters;

namespace EventStoreLearning.Appointment.QueryApi.Contract.Examples
{
    internal class AppointmentListExamples : IExamplesProvider<IList<Appointment>>, IExamplesProvider<ErrorResponse>
    {
        IList<Appointment> IExamplesProvider<IList<Appointment>>.GetExamples()
        {
            return new List<Appointment>()
            {
                new Appointment
                {
                    Id = Guid.NewGuid(),
                    Title = "Sample Meeting #1",
                    StartTime = DateTime.UtcNow,
                    DurationMinutes = 30
                },
                new Appointment
                {
                    Id = Guid.NewGuid(),
                    Title = "Lunch",
                    StartTime = DateTime.Today.AddHours(12),
                    DurationMinutes = 60
                },
                new Appointment
                {
                    Id = Guid.NewGuid(),
                    Title = "Another Meeting #2",
                    StartTime = DateTime.UtcNow,
                    DurationMinutes = 90
                }
            };
        }

        ErrorResponse IExamplesProvider<ErrorResponse>.GetExamples()
        {
            return new ErrorResponse("There was a problem while trying to get appointments");
        }
    }
}
