using Newtonsoft.Json;

namespace AwesomeEventGrid.Abstractions.Models
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