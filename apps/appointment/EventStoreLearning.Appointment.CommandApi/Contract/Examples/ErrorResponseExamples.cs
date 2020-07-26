using System;
using EventStoreLearning.Appointment.Commands;
using EventStoreLearning.Common.Web.Models;
using Swashbuckle.AspNetCore.Filters;

namespace EventStoreLearning.Appointment.CommandApi.Contract.Examples
{
    public class ErrorResponseExamples : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples()
        {
            return new ErrorResponse("There was a problem while trying to perform your request");
        }
    }
}
