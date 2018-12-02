using AutoMapper;
using AwesomeEventGrid.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace AwesomeEventGrid.Endpoints
{
    public class TopicsGetAllEndpoint: EndpointBase
    {
        public TopicsGetAllEndpoint(RequestDelegate next)
        {

        }


        public async Task InvokeAsync(HttpContext context, ITopicsRepository topicsRepository, IMapper mapper)
        {

            var topics = topicsRepository.GetAll();
            var model = mapper.Map<IEnumerable<TopicModel>>(topics);

            await Ok(context, model);

        }
    }
}
