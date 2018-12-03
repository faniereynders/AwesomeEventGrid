using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using AwesomeEventGrid.Abstractions.Models;

namespace Consumer1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> logger;
        public EventsController(ILoggerFactory loggerFactory)
        {
            var a = new HttpClient();
           
            this.logger = loggerFactory.CreateLogger<EventsController>();
        }

        // POST api/values
        [HttpPost("in", Name = "Events")]
        //[ValidateHook]
        public IActionResult Post([FromBody] EventModel @event)
        {
            //@event.Part
            

                if (@event.EventType == "subscriptions.validate")
            {
                var eventData = ((JObject)(@event.Data)).ToObject<SubscriptionValidationEventDataModel>();
                this.logger.LogInformation($"Got SubscriptionValidation event data, validation code: {eventData.ValidationCode}, source: {@event.Source}");
                // Do any additional validation (as required) and then return back the below response

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Subscription validation: \r\n {JsonConvert.SerializeObject(@event)}");
                Console.ResetColor();

                var responseData = new SubscriptionValidationResponse()
                {
                    ValidationResponse = eventData.ValidationCode
                };

                return Ok(responseData);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Event received: \r\n {JsonConvert.SerializeObject(@event)}");
            Console.ResetColor();
           
            return Ok();
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
