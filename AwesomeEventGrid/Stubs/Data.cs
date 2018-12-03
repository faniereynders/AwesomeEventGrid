using AwesomeEventGrid.Entities;
using System.Collections.Generic;

namespace AwesomeEventGrid.Stubs
{
    public class Data
    {
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public List<Topic> Topics { get; set; } = new List<Topic>();

        public Data()
        {
            Subscriptions
                .Add(new Subscription
                {
                    EndpointUrl = "https://localhost:5001/api/diagnostics",
                    EventTypes = new string[] { "events.deliveryFailed" },
                    Topic = ""
                });

            Topics.Add(new Topic
            {
                Name = "foo"
            });
        }
    }
}
