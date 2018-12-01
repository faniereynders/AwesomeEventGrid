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
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace AwesomeEventGrid.Endpoints
{
    public class CreateTopicEndpoint : EndpointBase
    {

        public CreateTopicEndpoint(RequestDelegate next)
        {

        }

        

        public async Task InvokeAsync(HttpContext context, ITopicsRepository topicsRepository, IMapper mapper, DefaultEventGridEventHandler eventHandler, IOptions<AwesomeEventGridOptions> options)
        {
            try
            {
                ModelState.Reset();

                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    var topicToCreate = JsonConvert.DeserializeObject<TopicModel>(await reader.ReadToEndAsync(), options.Value.SerializerSettings);
                    if (topicToCreate == null)
                    {
                        ModelState.AddError("", "Request body is required");
                    }

                    Validate(topicToCreate);

                    if (!ModelState.IsValid)
                    {
                        await BadRequest(context);

                        return;
                    }

                    if (topicsRepository.FindByName(topicToCreate.Name) != null)
                    {
                        ModelState.AddError("name", "Topic does already exists");
                        await BadRequest(context);
                        return;
                    }

                    var topic = mapper.Map<Topic>(topicToCreate);
                    topic = topicsRepository.Add(topic);

                    var topicModel = mapper.Map<TopicModel>(topic);
                    //todo fix url:
                    await CreatedAt(context, "http://foo", topicModel);

                }
            }
            catch (JsonException ex)
            {

                ModelState.AddError("", ex.Message);
                await BadRequest(context);

            }
            



        }

       
    }
}
