using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using AwesomeEventGrid.Abstractions;
using AwesomeEventGrid.Abstractions.Models;
using AwesomeEventGrid.Entities;
using Hangfire;

namespace AwesomeEventGrid.Infrastructure
{
    public class HangfireSubscriberEventDispatcher : ISubscriberEventDispatcher
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HangfireSubscriberEventDispatcher(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }


        public Task Dispatch(SubscriptionModel subscription, EventModel @event)
        {

            BackgroundJob.Enqueue(() => SendEventToSubscriber(subscription, @event));
            return Task.CompletedTask;

        }

        [Queue("events"), AutomaticRetry(Attempts = 3)]
        public async Task SendEventToSubscriber(SubscriptionModel subscription, EventModel @event)
        {
            var client = httpClientFactory.CreateClient();

            var response = await client.PostAsync(subscription.EndpointUrl, @event, new JsonMediaTypeFormatter());
            response.EnsureSuccessStatusCode();
        }
    }



}
