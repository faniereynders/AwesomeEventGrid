using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AwesomeEventGrid.Abstractions.Options
{
    public class AwesomeEventGridOptions
    {
        public AwesomeEventGridOptions()
        {
            SerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
        public string BasePath { get; set; } = "Awesome.EventGrid";
        public string TopicsPath { get; set; } = $"topics";
        //public string EventsPath { get; set; } = "topics/{topic}";
        public string SubscriptionsPath { get; set; } = "subscriptions";
        public JsonSerializerSettings SerializerSettings { get; set; }
    }
}
