using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventStoreLearning.Appointment.CommandApi.Controllers
{
    /// <summary>
    /// Endpoints dealing with the status of the API
    /// </summary>
    [Route("[controller]")]
    public class HealthController : Controller
    {
        /// <summary>
        /// A simple endpoint for testing the status of the API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(new { status = "Running" }) { StatusCode = 200 };
        }
    }
}
