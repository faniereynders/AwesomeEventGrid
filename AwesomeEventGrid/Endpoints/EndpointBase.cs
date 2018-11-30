using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeEventGrid.Endpoints
{
    public abstract class EndpointBase
    {
        public async Task Ok(HttpContext context, object model)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(model));
        }
        public Task Accepted(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 202;
            return Task.CompletedTask;
        }

        public async Task CreatedAt(HttpContext context, string url, object model)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 201;
            context.Response.Headers.Add("Link", url);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(model));
            
        }
        public Task BadRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 400;
            return Task.CompletedTask;
        }

        public Task NotFound(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 404;
            
            return Task.CompletedTask;
        }
    }
}
