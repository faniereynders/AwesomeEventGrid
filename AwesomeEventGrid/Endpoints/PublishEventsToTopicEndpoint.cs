using AutoMapper;
using AwesomeEventGrid.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using System.IO;
using AwesomeEventGrid.Infrastructure;
using System;
using Microsoft.Extensions.Options;

namespace AwesomeEventGrid.Endpoints
{
    public class PublishEventsToTopicEndpoint : EndpointBase
    {

        public PublishEventsToTopicEndpoint(RequestDelegate next)
        {

        }

        public async Task InvokeAsync(HttpContext context, ITopicsRepository topicsRepository, IMapper mapper, DefaultEventGridEventHandler eventHandler)
        {
            var options = context.RequestServices.GetService<IOptions<AwesomeEventGridOptions>>();
            ModelState.Reset();
            var topic = (string)context.GetRouteData().Values["topic"];
            
            if (topicsRepository.FindByName(topic) == null)
            {
                ModelState.AddError("name", "Topic does not exists");
                await BadRequest(context);
            }
            else
            {
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    var eventsToPublish = JsonConvert.DeserializeObject<EventModel[]>(await reader.ReadToEndAsync(), options.Value.SerializerSettings);
                    eventHandler.Handle(topic, eventsToPublish);

                    await Accepted(context);

                }
            }
            



        }

       
    }
}
