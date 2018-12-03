using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using AwesomeEventGrid.Abstractions;
using AwesomeEventGrid.Abstractions.Models;
using Hangfire;

namespace AwesomeEventGrid.Hangfire
{
    public class HangfireSubscriberEventDispatcher : ISubscriberEventDispatcher
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HangfireSubscriberEventDispatcher(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        
        public Task Dispatch(ISubscription subscription, EventModel @event)
        {
            BackgroundJob.Enqueue(() => SendEventToSubscriber(subscription.EndpointUrl, @event));
            return Task.CompletedTask;

        }

        [Queue("events"), AutomaticRetry(Attempts = 3)]
        public async Task SendEventToSubscriber(string endpointUrl, EventModel @event)
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.PostAsync(endpointUrl, @event, new JsonMediaTypeFormatter());
            response.EnsureSuccessStatusCode();
        }
    }



}
