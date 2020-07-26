using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModel.Queries;
using EventStoreLearning.Appointment.ReadModels.Models;
using Microsoft.AspNetCore.Mvc;
using EventStoreLearning.Common.Web;
using EventStoreLearning.Common.Web.Extensions;

namespace EventStoreLearning.Appointment.QueryApi.Controllers
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
        /// <param name="dataContractFactory"></param>
        public AppointmentController(IMediatedDataContractFactory dataContractFactory)
        {
            _dataContractFactory = dataContractFactory;
        }

        /// <summary>
        /// Get all appointments
        /// </summary>
        /// <returns>A list of appointments</returns>
        /// <response code="200">Returns a list of appointments</response>
        /// <response code="400">Returns an exception if there is a problem</response>
        [HttpGet]
        public async Task<IList<Contract.Appointment>> GetAll()
        {
            var test = await _dataContractFactory.CreateContract<IList<Contract.Appointment>>()
                .Mediate<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>>()
                .Invoke();
                //.CreateApiResponse();

            return test;
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(Guid id)
        //{
        //    return "value";
        //}
    }
}
