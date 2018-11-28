using System.Collections.Generic;

namespace WebApplication23
{
    public class Data
    {
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public List<Topic> Topics { get; set; } = new List<Topic>();

        public Data()
        {
            Subscriptions.Add(new Subscription
            {
                EndpointUrl = "https://localhost:5001/api/diagnostics",
                EventTypes = new string[] { "Events.EventDeliveryFailed" },
                //Topic = "tmt-test"
            });
        }
    }
}
