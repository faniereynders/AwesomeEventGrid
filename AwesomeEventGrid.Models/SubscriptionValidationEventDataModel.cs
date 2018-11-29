using Newtonsoft.Json;

namespace AwesomeEventGrid.Models
{
    public class SubscriptionValidationEventDataModel
    {
        [JsonProperty("validationCode")]
        public string ValidationCode
        {
            get; set;
        }
    }
}