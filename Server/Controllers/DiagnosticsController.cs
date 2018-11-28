using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication23.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticsController : ControllerBase
    {
        private readonly EventHandler eventHandler;

        public DiagnosticsController(EventHandler eventHandler)
        {
            this.eventHandler = eventHandler;
        }

        // POST api/values
        [HttpPost(Name = "DiagnosticsHandler")]
        public IActionResult Post([FromBody] EventModel diagnosticEvent)
        {
            

            return Ok();
        }


    }
}
