using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModel;
using EventStoreLearning.Appointment.ReadModel.Queries;
using EventStoreLearning.Appointment.ReadModels.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EventStoreLearning.Appointment.QueryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IAppointmentQueryMediator _queryMediator;

        public ValuesController(IAppointmentQueryMediator queryMediator)
        {
            _queryMediator = queryMediator;
        }

        // GET api/values
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var query = new RetrieveAllAppointmentsQuery();
            var tokenSource = new CancellationTokenSource();

            var response = await _queryMediator.Query<RetrieveAllAppointmentsQuery, IList<AppointmentReadModel>>(query, tokenSource.Token);

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            return response.Error == null
                   ? new JsonResult(response.Response, settings) { StatusCode = 200 }
                   : new JsonResult(response.Error, settings) { StatusCode = 400 };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
