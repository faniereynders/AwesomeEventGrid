using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AwesomeEventGrid.Infrastructure
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
        public string TopicsResourcePath { get; set; }
        public JsonSerializerSettings SerializerSettings { get; set; }
    }
}