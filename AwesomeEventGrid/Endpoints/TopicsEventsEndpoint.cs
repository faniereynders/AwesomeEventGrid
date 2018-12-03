using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using AwesomeEventGrid.Abstractions.Options;
using AwesomeEventGrid.Abstractions;
using AwesomeEventGrid.Abstractions.Models;

namespace AwesomeEventGrid.Endpoints
{
    public class TopicsEventsEndpoint : EndpointBase
    {

        public TopicsEventsEndpoint(RequestDelegate next)
        {

        }

        public async Task InvokeAsync(HttpContext context, ITopicsRepository topicsRepository, IMapper mapper, IEventGridEventHandler eventGridEventHandler)
        {
            var options = context.RequestServices.GetService<IOptions<AwesomeEventGridOptions>>();
            ModelState.Reset();
            var topic = (string)context.GetRouteData().Values["name"];
            
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
                    await eventGridEventHandler.HandleAsync(topic, eventsToPublish);

                    await Accepted(context);

                }
            }
            



        }

       
    }
}
