using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventStoreLearning.Appointment.CommandApi.Contract;
using EventStoreLearning.Appointment.Commands;
using Microsoft.AspNetCore.Http;
using EventStoreLearning.Common.Web;
using System;
using EventStoreLearning.Common.Web.Extensions;

namespace EventStoreLearning.Appointment.CommandApi.Controllers
{
    /// <summary>
    /// Controller for all Appointment related endpoints
    /// </summary>
    [Route("v1/[controller]")]
    public class AppointmentController : Controller
    {
        private readonly IMediatedDataContractFactory _dataContractFactory;

        /// <summary>
        /// Ctor params provided by DI...
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        public AppointmentController(IMediatedDataContractFactory dataContractFactory)
        {
            _dataContractFactory = dataContractFactory;
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
        /// <param name="request">The request containing info needed to create an appointment</param>
        /// <returns>The ID of the appointment created</returns>
        /// <response code="201">When successful, returns the ID assigned of the appointment created</response>
        /// <response code="400">Returns an error response if there's a problem with the request</response>
        /// <response code="401">Returns an error response if the user isn't allowed to make this request</response>
        /// <response code="403">Returns an error response if the user hasn't authenticated</response>
        /// <response code="404">Returns an error response if a dependency of the request wasn't found</response>
        /// <response code="500">Returns an error response if there's a problem with the current environment</response>
        /// <response code="503">Returns an error response if there's a temporary issue with the current environment</response>
        /// <response code="509">Returns an error response if a conflict arises while attempting to fulfill the request</response>
        [HttpPost]
        public async Task<Guid> Post(CreateAppointmentRequest request)
        {
            var response = await _dataContractFactory.CreateContract<CreateAppointmentRequest, Guid>()
                .Mediate<CreateAppointmentCommand>()
                .Invoke(request);
                //.CreateApiResponse(StatusCodes.Status201Created);

            return response;
        }

        /// <summary>
        /// Change an appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /v1/appointment
        ///     {
        ///         "id": "4556a817-80cc-4d1f-bb77-9fa64da21326",
        ///         "version": 2,
        ///         "title": "Sample Meeting #1", //optional
        ///         "startTime": "2019-11-07T01:28:10.093Z", //optional
        ///         "durationMinutes": 30 //optional
        ///     }
        ///
        /// </remarks>
        /// <param name="request">The request containing info needed to change an appointment</param>
        /// <returns>The ID of the appointment changed</returns>
        /// <response code="200">When successful, returns the ID assigned of the appointment changed</response>
        /// <response code="400">Returns an error response if there's a problem with the request</response>
        /// <response code="401">Returns an error response if the user isn't allowed to make this request</response>
        /// <response code="403">Returns an error response if the user hasn't authenticated</response>
        /// <response code="404">Returns an error response if a dependency of the request wasn't found</response>
        /// <response code="500">Returns an error response if there's a problem with the current environment</response>
        /// <response code="503">Returns an error response if there's a temporary issue with the current environment</response>
        /// <response code="509">Returns an error response if a conflict arises while attempting to fulfill the request</response>
        [HttpPut("{id}")]
        public async Task<Guid> Put(ChangeAppointmentRequest request)
        {
            var response = await _dataContractFactory.CreateContract<ChangeAppointmentRequest, Guid>()
                .Mediate<ChangeAppointmentCommand>()
                .Invoke(request);
                //.CreateApiResponse();

            return response;
        }

        /// <summary>
        /// Cancel an appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /v1/appointment
        ///     {
        ///         "id": "4556a817-80cc-4d1f-bb77-9fa64da21326",
        ///         "version": 2,
        ///     }
        ///
        /// </remarks>
        /// <param name="request">The request containing info needed to cancel an appointment</param>
        /// <returns>The ID of the appointment cancelled</returns>
        /// <response code="200">When successful, returns the ID assigned of the appointment cancelled</response>
        /// <response code="400">Returns an error response if there's a problem with the request</response>
        /// <response code="401">Returns an error response if the user isn't allowed to make this request</response>
        /// <response code="403">Returns an error response if the user hasn't authenticated</response>
        /// <response code="404">Returns an error response if a dependency of the request wasn't found</response>
        /// <response code="500">Returns an error response if there's a problem with the current environment</response>
        /// <response code="503">Returns an error response if there's a temporary issue with the current environment</response>
        /// <response code="509">Returns an error response if a conflict arises while attempting to fulfill the request</response>
        [HttpDelete("{id}")]
        public async Task<Guid> Delete(CancelAppointmentRequest request)
        {
            var response = await _dataContractFactory.CreateContract<CancelAppointmentRequest, Guid>()
                .Mediate<CancelAppointmentCommand>()
                .Invoke(request);
                //.CreateApiResponse();

            return response;
        }
    }
}
