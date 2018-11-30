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
using AwesomeEventGrid.Entities;

namespace AwesomeEventGrid.Endpoints
{
    public class CreateTopicEndpoint : EndpointBase
    {

        public CreateTopicEndpoint(RequestDelegate next)
        {

        }

        public async Task InvokeAsync(HttpContext context, ITopicsRepository topicsRepository, IMapper mapper, DefaultEventGridEventHandler eventHandler)
        {
            var name = (string)context.GetRouteData().Values["name"];
            
            if (topicsRepository.FindByName(name) != null)
            {
                //todo ModelState.AddModelError("name", "Topic does already exists");
                await BadRequest(context);
            }
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                var topicToCreate = JsonConvert.DeserializeObject<TopicModel>(await reader.ReadToEndAsync());
                var topic = mapper.Map<Topic>(topicToCreate);
                topic = topicsRepository.Add(topic);

                var topicModel = mapper.Map<TopicModel>(topic);
                //todo fix url:
                await CreatedAt(context, "http://foo", topicModel);

            }



        }

       
    }
}
