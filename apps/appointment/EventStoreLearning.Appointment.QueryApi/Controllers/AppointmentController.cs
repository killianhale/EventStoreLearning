using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contract = EventStoreLearning.Appointment.QueryApi.Contract;
using EventStoreLearning.Appointment.QueryApi.Contract.Examples;
using EventStoreLearning.Appointment.ReadModel;
using EventStoreLearning.Appointment.ReadModel.Queries;
using EventStoreLearning.Appointment.ReadModels.Models;
using EventStoreLearning.Common.Web.Extensions;
using EventStoreLearning.Common.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using EventStoreLearning.Common.Querying;
using EventStoreLearning.Common.Web;

namespace EventStoreLearning.Appointment.QueryApi.Controllers
{
    /// <summary>
    /// Controller for all Appointment related endpoints
    /// </summary>
    [Route("v1/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(ErrorResponse))]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public class AppointmentController : BaseQueryController
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Ctor params provided by DI...
        /// </summary>
        /// <param name="queryMediator"></param>
        /// <param name="mapper"></param>
        public AppointmentController(IQuery queryMediator, IMapper mapper)
            : base(queryMediator)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Get all appointments
        /// </summary>
        /// <returns>A list of appointments</returns>
        /// <response code="200">Returns a list of appintments</response>
        /// <response code="400">Returns an exception if there is a problem</response>
        [HttpGet]
        [ProducesResponseType(typeof(IList<Contract.Appointment>), StatusCodes.Status200OK)]
        [SwaggerRequestExample(typeof(IList<Contract.Appointment>), typeof(AppointmentListExamples))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AppointmentListExamples))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(AppointmentListExamples))]
        public async Task<JsonResult> Get()
        {
            //var query = _mapper.Map<CreateAppointmentCommand>(request);
            var query = new RetrieveAllAppointmentsQuery();

            var queryResponse = await PublishQuery<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>> (query);
            var response = queryResponse.AsApiResponse<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>, IList<Contract.Appointment>>(200, 400, _mapper);

            return response;
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(Guid id)
        //{
        //    return "value";
        //}
    }
}
