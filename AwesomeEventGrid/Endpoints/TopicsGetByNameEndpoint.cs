using AutoMapper;
using AwesomeEventGrid.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace AwesomeEventGrid.Endpoints
{
    public class TopicsGetByNameEndpoint: EndpointBase
    {

        public TopicsGetByNameEndpoint(RequestDelegate next)
        {

        }

        public async Task InvokeAsync(HttpContext context, ITopicsRepository topicsRepository, IMapper mapper)
        {
            ModelState.Reset();

            var name = (string)context.GetRouteData().Values["name"];
            var topic = topicsRepository.FindByName(name);
            if (topic == null)
            {
                ModelState.AddError("name", "Topic with this name not found");
                await NotFound(context);
            }
            else
            {
                var model = mapper.Map<TopicModel>(topic);
                await Ok(context, model);
            }
            

        }
    }
}
