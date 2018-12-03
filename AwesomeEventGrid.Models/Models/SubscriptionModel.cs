using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AwesomeEventGrid.Abstractions.Models
{
    public class SubscriptionModel : ISubscription
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string EndpointUrl { get; set; }
        public string Topic { get; set; }
        public string[] EventTypes { get; set; }
        public string SubjectBeginsWith { get; set; }
        public string SubjectEndsWith { get; set; }

        //public const string RouteName = "Topics.GetByName";

    }
}