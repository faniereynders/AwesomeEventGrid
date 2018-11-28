using Newtonsoft.Json;

namespace Consumer1.Controllers
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