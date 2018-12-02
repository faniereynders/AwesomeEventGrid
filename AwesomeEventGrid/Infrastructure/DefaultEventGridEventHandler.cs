using AwesomeEventGrid.Models;
using Hangfire;
using System;
using System.Linq;

namespace AwesomeEventGrid.Infrastructure
{
    public class DefaultEventGridEventHandler
    {
        private readonly ISubscriptionsRepository subscriptionsRepository;
        private readonly SubscriberDispatcher subscriberDispatcher;

        public DefaultEventGridEventHandler(ISubscriptionsRepository subscriptionsRepository, SubscriberDispatcher subscriberDispatcher)
        {
            this.subscriptionsRepository = subscriptionsRepository;
            this.subscriberDispatcher = subscriberDispatcher;
        }
        public void Handle(string topic, params EventModel[] events)
        {
            foreach (var @event in events)
            {
                var subs = subscriptionsRepository.GetAll(topic).Where(s => s.EventTypes == null || s.EventTypes.Contains(@event.EventType));
                foreach (var subscriber in subs)
                {
                    
                    BackgroundJob.Enqueue(() => subscriberDispatcher.Dispatch(subscriber, @event));
                }
            }
        }
    }
}
