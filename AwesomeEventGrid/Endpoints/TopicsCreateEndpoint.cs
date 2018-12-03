using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AwesomeEventGrid.Infrastructure;
using AwesomeEventGrid.Entities;
using Microsoft.Extensions.Options;
using AwesomeEventGrid.Abstractions.Models;
using AwesomeEventGrid.Abstractions.Options;

namespace AwesomeEventGrid.Endpoints
{
    public class TopicsCreateEndpoint : EndpointBase
    {

        public TopicsCreateEndpoint(RequestDelegate next)
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
