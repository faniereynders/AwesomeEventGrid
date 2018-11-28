using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication23
{
    //public class Event
    //{
    //    public string Topic { get; set; }
    //    public string Subject { get; set; }
    //    public string EventType { get; set; }
    //    public DateTime EventTime { get; set; } = DateTime.Now;
    //    public string Id { get; set; } = Guid.NewGuid().ToString();
    //    public object Data { get; set; }
    //    public string DataVersion { get; set; } = "1.0";
    //    public string MetadataVersion { get; set; } = "1.0";
    //}
    public class EventModel
    {
        [JsonProperty("eventType")]
        [JsonRequired, Required]
        public string EventType { get; set; }
        [JsonProperty("eventTypeVersion")]
        public string EventTypeVersion { get; set; } = "1.0";
        [JsonProperty("cloudEventsVersion")]
        public string CloudEventsVersion { get; set; } = "0.1";
        [JsonProperty("source")]
        [JsonRequired, Required]
        public Uri Source { get; set; }
        [JsonProperty("eventID")]
        
        public string EventID { get; set; } = Guid.NewGuid().ToString();
        [JsonProperty("eventTime")]
        public DateTime EventTime { get; set; } = DateTime.Now;
        [JsonProperty("schemaURI")]
        public Uri SchemaURI { get; set; }
        [JsonProperty("contentType")]
        public string ContentType { get; set; } = "application/json";
        [JsonProperty("extensions")]
        public Dictionary<string,object> Extensions { get; set; }
        [JsonProperty("data")]
        public object Data { get; set; }
    }


}
