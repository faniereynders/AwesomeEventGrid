using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication23
{
    public class Subscription
    {
        public string Name { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EndpointUrl { get; set; }
        public string Topic { get; set; }
        public string[] EventTypes { get; set; }
        public string SubjectBeginsWith { get; set; }
        public string SubjectEndsWith { get; set; }
    }

    public class SubscriptionModel : Subscription
    {
        public const string RouteName = "Topics.GetByName";

        public string Id { get; set; }
        [Required]
        public new string EndpointUrl { get; set; }
        [Required]
        public new string Name { get; set; }
    }
}