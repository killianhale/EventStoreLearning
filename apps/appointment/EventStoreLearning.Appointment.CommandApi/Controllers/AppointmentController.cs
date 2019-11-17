using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EventStoreLearning.Appointment.CommandApi.Contract;
using EventStoreLearning.Appointment.CommandApi.Contract.Examples;
using EventStoreLearning.Common.Web;
using EventStoreLearning.Common.Web.Models;
using EventStoreLearning.Common.Web.Extensions;
using EventStoreLearning.Appointment.Commands;
using EventStoreLearning.Common.EventSourcing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace EventStoreLearning.Appointment.CommandApi.Controllers
{
    /// <summary>
    /// Controller for all Appointment related endpoints
    /// </summary>
    [Route("v1/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(ErrorResponse))]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public class AppointmentController : BaseCommandController
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Ctor params provided by DI...
        /// </summary>
        /// <param name="commandMediator"></param>
        /// <param name="mapper"></param>
        public AppointmentController(ICommandMediator commandMediator, IMapper mapper)
            : base(commandMediator)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Create an appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /v1/appointment
        ///     {
        ///         "title": "Sample Meeting #1",
        ///         "startTime": "2019-11-07T01:28:10.093Z",
        ///         "durationMinutes": 30
        ///     }
        ///
        /// </remarks>
        /// <param name="request">The request containing info needed to create appointment</param>
        /// <returns>A a success message</returns>
        /// <response code="201">Returns a success message</response>
        /// <response code="409">Returns an exception if there is a confict or error</response>  
        [HttpPost]
        [ProducesResponseType(typeof(CreateAppointmentCommand), StatusCodes.Status201Created)]
        [SwaggerRequestExample(typeof(CreateAppointmentRequest), typeof(CreateAppointmentCommandExamples))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(CreateAppointmentCommandExamples))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(CreateAppointmentCommandExamples))]
        public async Task<JsonResult> Post([FromBody]CreateAppointmentRequest request)
        {
            var command = _mapper.Map<CreateAppointmentCommand>(request);

            var cmdResponse = await PublishCommand(command);
            var response = cmdResponse.AsApiResponse(201, 409);

            return response;
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]Appointment value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
