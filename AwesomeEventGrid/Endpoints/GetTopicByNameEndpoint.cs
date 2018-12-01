using AutoMapper;
using AwesomeEventGrid.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;

namespace AwesomeEventGrid.Endpoints
{
    public class GetTopicByNameEndpoint: EndpointBase
    {

        public GetTopicByNameEndpoint(RequestDelegate next)
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
