using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using AwesomeEventGrid.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using AwesomeEventGrid.Abstractions.Models;
using AwesomeEventGrid.Abstractions.Options;

namespace AwesomeEventGrid.Endpoints
{
    public class SubscriptionsGetByNameEndpoint : EndpointBase
    {

        public SubscriptionsGetByNameEndpoint(RequestDelegate next)
        {

        }

        

        public async Task InvokeAsync(HttpContext context, ISubscriptionsRepository subscriptionsRepository, IMapper mapper, DefaultEventGridEventHandler eventHandler, IOptions<AwesomeEventGridOptions> options)
        {
            ModelState.Reset();
            var routeData = context.GetRouteData();
            var topic = (string)routeData.Values["topic"];
            var name = (string)routeData.Values["name"];
            var subscription = subscriptionsRepository.FindByName(topic, name);
            if (subscription == null)
            {
                ModelState.AddError("name", $"Subscription with this name not found for topic '{topic}'");
                await NotFound(context);
                return;
            }

            var model = mapper.Map<SubscriptionModel>(subscription);

            await Ok(context, model);




        }

       
    }
}
