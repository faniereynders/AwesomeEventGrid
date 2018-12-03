using AutoMapper;
using AwesomeEventGrid.Abstractions;
using AwesomeEventGrid.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeEventGrid.Infrastructure
{
    public class DefaultEventGridEventHandler : IEventGridEventHandler
    {
        private readonly ISubscriptionsRepository subscriptionsRepository;
        private readonly ISubscriberEventDispatcher subscriberEventDispatcher;
        private readonly IMapper mapper;

        public DefaultEventGridEventHandler(ISubscriptionsRepository subscriptionsRepository, ISubscriberEventDispatcher subscriberEventDispatcher, IMapper mapper)
        {
            this.subscriptionsRepository = subscriptionsRepository;
            this.subscriberEventDispatcher = subscriberEventDispatcher;
            this.mapper = mapper;
        }
        public async Task HandleAsync(string topic, params EventModel[] events)
        {
            var dispatcherTasks = new List<Task>();

            foreach (var @event in events)
            {
                var subscriptions = subscriptionsRepository.GetAll(topic)
                    .Where(s => s.EventTypes == null || s.EventTypes.Contains(@event.EventType))
                    .Select(s => subscriberEventDispatcher.Dispatch(mapper.Map<SubscriptionModel>(s), @event));

                dispatcherTasks.AddRange(subscriptions);
            }

            await Task.WhenAll(dispatcherTasks);
        }
    }
}
