using AwesomeEventGrid.Abstractions;
using System;

namespace AwesomeEventGrid.Entities
{
    public class Subscription : ISubscription
    {
        public string Name { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EndpointUrl { get; set; }
        public string Topic { get; set; }
        public string[] EventTypes { get; set; }
        public string SubjectBeginsWith { get; set; }
        public string SubjectEndsWith { get; set; }
    }
}