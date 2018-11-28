using Newtonsoft.Json;

namespace WebApplication23.Controllers
{
    public class SubscriptionValidationEventData
    {
        [JsonProperty("validationCode")]
        public string ValidationCode
        {
            get; set;
        }
    }
}