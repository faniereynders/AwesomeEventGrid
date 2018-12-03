using AwesomeEventGrid.Abstractions.Models;
using AwesomeEventGrid.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication23.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticsController : ControllerBase
    {

       

        // POST api/values
        [HttpPost(Name = "DiagnosticsHandler")]
        public IActionResult Post([FromBody] EventModel diagnosticEvent)
        {
            

            return Ok();
        }


    }
}
