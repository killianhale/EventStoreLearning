using Microsoft.AspNetCore.Mvc;


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
