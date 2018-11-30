using AutoMapper;
using AwesomeEventGrid.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
namespace AwesomeEventGrid.Endpoints
{
    public class GetAllTopicsEndpoint: EndpointBase
    {
        public GetAllTopicsEndpoint(RequestDelegate next)
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
