﻿using System;
using EventStoreLearning.Appointment.Commands;
using EventStoreLearning.Common.Web.Models;
using Swashbuckle.AspNetCore.Filters;

namespace EventStoreLearning.Appointment.CommandApi.Contract.Examples
{
    public class CreateAppointmentRequestExamples : IExamplesProvider<CreateAppointmentRequest>
    {
        public CreateAppointmentRequest GetExamples()
        {
            return new CreateAppointmentRequest
            {
                Title = "Sample Meeting #1",
                StartTime = DateTime.UtcNow,
                DurationMinutes = 30
            };
        }
    }
}
