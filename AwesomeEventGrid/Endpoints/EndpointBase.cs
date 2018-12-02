using AwesomeEventGrid.Infrastructure;
using AwesomeEventGrid.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AwesomeEventGrid.Endpoints
{
    public abstract class EndpointBase
    {

        public EndpointBase()
        {
            ModelState.Reset();
        }
        public ModelState ModelState { get; set; } = new ModelState();

        protected void Validate<TModel>(TModel model) where TModel : IValidatableObject
        {
            if (model == null)
            {
                return;
            }
            var results = model.Validate(new System.ComponentModel.DataAnnotations.ValidationContext(model));
            foreach (var result in results)
            {
                foreach (var member in result.MemberNames)
                {
                    ModelState.AddError(member, result.ErrorMessage);
                }
            }
        }

        public async Task Ok(HttpContext context, object model)
        {
            var options = context.RequestServices.GetService<IOptions<AwesomeEventGridOptions>>();
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(model, options.Value.SerializerSettings));
        }
        public Task Accepted(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 202;
            return Task.CompletedTask;
        }

        //public async Task Created(HttpContext context, string url, object model)
        //{
        //    var options = context.RequestServices.GetService<IOptions<AwesomeEventGridOptions>>();
        //    context.Response.ContentType = "application/json; charset=utf-8";
        //    context.Response.StatusCode = 200;
        //    context.Response.Headers.Add("Link", url);
        //    await context.Response.WriteAsync(JsonConvert.SerializeObject(model, options.Value.SerializerSettings));
        //}

        public async Task CreatedAt(HttpContext context, string url, object model)
        {
            var options = context.RequestServices.GetService<IOptions<AwesomeEventGridOptions>>();
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 201;
            context.Response.Headers.Add("Link", url);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(model, options.Value.SerializerSettings));

        }
        public async Task BadRequest(HttpContext context)
        {
            var options = context.RequestServices.GetService<IOptions<AwesomeEventGridOptions>>();
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 400;

            if (ModelState != null)
            {
                var model = new ValidationProblemDetailsModel(ModelState);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(model, options.Value.SerializerSettings));
            }

        }

        public async Task NotFound(HttpContext context)
        {
            var options = context.RequestServices.GetService<IOptions<AwesomeEventGridOptions>>();
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 404;

            if (ModelState != null)
            {
                var model = new ValidationProblemDetailsModel(ModelState);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(model, options.Value.SerializerSettings));
            }
        }
    }
}
