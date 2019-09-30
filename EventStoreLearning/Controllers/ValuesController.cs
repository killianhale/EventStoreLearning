using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentAggregate = EventStoreLearning.Appointment.Appointment;
using EventStoreLearning.Appointment;
using EventStoreLearning.Appointment.Events;
using EventStoreLearning.Common.EventSourcing;
using Microsoft.AspNetCore.Mvc;
using EventStoreLearning.Common.Logging;
using System.Threading;

namespace EventStoreLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ICommandMediator _commandMediator;

        public ValuesController(ICommandMediator commandMediator)
        {
            _commandMediator = commandMediator;
        }

        // GET api/values
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var command = new CreateAppointmentCommand(Guid.NewGuid(), "Some appointment", DateTime.Now, TimeSpan.FromHours(2));

            var tokenSource = new CancellationTokenSource();

            var response = await _commandMediator.PublishCommand(command, tokenSource.Token);

            return response.Error == null
                   ? new JsonResult(new { message = "Success!" }) { StatusCode = 201 }
                   : new JsonResult(response.Error) { StatusCode = 409 };
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
