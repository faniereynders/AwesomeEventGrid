using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AwesomeEventGrid.Models
{
    public class TopicModel
    {
        //public const string RouteName = "Topics.GetByName";
        //[JsonProperty("_self", Order = -1)]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}