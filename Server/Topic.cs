using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication23
{
    public class Topic
    {
        
        public Guid Id { get; set; } = Guid.NewGuid();


        public string Name { get; set; }
    }

    public class TopicModel: Topic
    {
        public const string RouteName = "Topics.GetByName";
        //[JsonProperty("_self", Order = -1)]
        public string Id { get; set; }

        [Required]
        public new string Name { get; set; }
    }
}