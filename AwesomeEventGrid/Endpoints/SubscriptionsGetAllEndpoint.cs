using AutoMapper;
using AwesomeEventGrid.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeEventGrid.Infrastructure;
using Microsoft.Extensions.Options;
using System.Linq;
using Microsoft.AspNetCore.Routing;

namespace AwesomeEventGrid.Endpoints
{
    public class SubscriptionsGetAllEndpoint : EndpointBase
    {

        public SubscriptionsGetAllEndpoint(RequestDelegate next)
        {

        }

        

        public async Task InvokeAsync(HttpContext context, ISubscriptionsRepository subscriptionsRepository, IMapper mapper, DefaultEventGridEventHandler eventHandler, IOptions<AwesomeEventGridOptions> options)
        {
            var routeData = context.GetRouteData();
            var topic = context.Request.Query["topic"].FirstOrDefault();
            var subscriptions = subscriptionsRepository.GetAll(topic);

            var model = mapper.Map<IEnumerable<SubscriptionModel>>(subscriptions);

            await Ok(context, model);

        }

       
    }
}
