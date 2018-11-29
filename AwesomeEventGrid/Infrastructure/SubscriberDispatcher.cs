using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using AwesomeEventGrid.Entities;
using AwesomeEventGrid.Models;
using Hangfire;

namespace AwesomeEventGrid.Infrastructure
{
    public class SubscriberDispatcher
    {
        private readonly IHttpClientFactory httpClientFactory;

        public SubscriberDispatcher(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [Queue("events"), AutomaticRetry(Attempts = 3)]
        public async Task Dispatch(Subscription subscription, EventModel @event)
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.PostAsync(subscription.EndpointUrl, @event, new JsonMediaTypeFormatter());
            response.EnsureSuccessStatusCode();
        }
    }



}
