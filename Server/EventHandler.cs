using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication23
{
    public class EventHandler
    {
        private readonly ISubscriptionsRepository subscriptionsRepository;
        private readonly SubscriberDispatcher subscriberDispatcher;

        public EventHandler(ISubscriptionsRepository subscriptionsRepository, SubscriberDispatcher subscriberDispatcher)
        {
            this.subscriptionsRepository = subscriptionsRepository;
            this.subscriberDispatcher = subscriberDispatcher;
        }
        public void Handle(string topic, params EventModel[] events)
        {
            foreach (var @event in events)
            {
                var subs = subscriptionsRepository.GetAll().Where(s => (s.Topic == null || s.Topic == topic) && (s.EventTypes == null || s.EventTypes.Contains(@event.EventType)));
                foreach (var subscriber in subs)
                {
                    
                    BackgroundJob.Enqueue(() => subscriberDispatcher.Dispatch(subscriber, @event));
                }
            }
        }
    }
}
