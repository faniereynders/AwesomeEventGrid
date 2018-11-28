using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Consumer1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> logger;
        public EventsController(ILoggerFactory loggerFactory)
        {
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
                var eventData = ((JObject)(@event.Data)).ToObject<SubscriptionValidationEventData>();
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

    public class EventModel
    {
        [JsonProperty("eventType")]
        [JsonRequired, Required]
        public string EventType
        {
            get; set;
        }
        [JsonProperty("eventTypeVersion")]
        public string EventTypeVersion { get; set; } = "1.0";
        [JsonProperty("cloudEventsVersion")]
        public string CloudEventsVersion { get; set; } = "0.1";
        [JsonProperty("source")]
        [JsonRequired, Required]
        public Uri Source
        {
            get; set;
        }
        [JsonProperty("eventID")]
        [JsonRequired, Required]
        public string EventID { get; set; } = Guid.NewGuid().ToString();
        [JsonProperty("eventTime")]
        public DateTime EventTime { get; set; } = DateTime.Now;
        [JsonProperty("schemaURI")]
        public Uri SchemaURI
        {
            get; set;
        }
        [JsonProperty("contentType")]
        public string ContentType { get; set; } = "application/json";
        [JsonProperty("extensions")]
        public Dictionary<string, object> Extensions
        {
            get; set;
        }
        [JsonProperty("data")]
        public object Data
        {
            get; set;
        }
    }
}
