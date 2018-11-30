using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using AwesomeEventGrid.Endpoints;

namespace AwesomeEventGrid.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAwesomeEventGrid(this IApplicationBuilder app)
        {
            

            var options = app.ApplicationServices.GetRequiredService<IOptions<EventGridOptions>>();
           // config.Bind(options);

            var routeHandler = new RouteHandler(context =>
            {
                var routeData = context.GetRouteData();
                var routeValues = routeData.Values;
                var route = routeData.Routers.OfType<Route>().First();

                switch (route.Name)
                {
                    case "EventGrid.GetTopics":
                        return context.Response.WriteAsync($"EventGrid.GetTopics");
                    case "EventGrid.GetTopicByName":
                        return context.Response.WriteAsync($"EventGrid.GetTopicByName");
                    
                    default:
                        break;
                }





                
                return context.Response.WriteAsync(
                    $"Hello! Route values: {string.Join(", ", routeValues)}");
            });

            var routeBuilder = new RouteBuilder(app, routeHandler);

            var topicsPath = $"{options.Value.BasePath}/{options.Value.TopicsPath}";

            routeBuilder.MapMiddlewareGet(topicsPath, b => b.UseMiddleware<GetAllTopicsEndpoint>());
            routeBuilder.MapMiddlewareGet(topicsPath + "/{name}", b => b.UseMiddleware<GetTopicByNameEndpoint>());
            routeBuilder.MapMiddlewarePost(topicsPath, b => b.UseMiddleware<CreateTopicEndpoint>());

            var eventsPath = $"{options.Value.BasePath}/{options.Value.EventsPath}";
            routeBuilder.MapMiddlewarePost(eventsPath, b => b.UseMiddleware<PublishEventsToTopicEndpoint>());


            routeBuilder.MapGet(
                topicsPath, async (context) => await context.Response.WriteAsync($"EventGrid.GetAllTopics")
            );
            routeBuilder.MapGet(topicsPath + "/{name}", async (context) => {
                var name = context.GetRouteData().Values["name"];

                await context.Response.WriteAsync($"EventGrid.GetTopicByName: {name}");
            });
            routeBuilder.MapPost(
                topicsPath, async (context) => await context.Response.WriteAsync($"EventGrid.CreateTopic")
            );

            routeBuilder.MapPut(
               topicsPath, async (context) => await context.Response.WriteAsync($"EventGrid.UpdateTopic")
           );
           

         

            routeBuilder.MapRoute(
                "EventGrid.GetTopicByName",
                "_api/topics/{name}"

            );


            var routes = routeBuilder.Build();
            app.UseRouter(routes);



            GlobalJobFilters.Filters.Add(new ManagedStateFilterAttribute(new HangfireActivator(app.ApplicationServices)));
            var hangfireOptions = new BackgroundJobServerOptions
            {
                Queues = new[] { "events" },
                //  Activator = new HangfireActivator(serviceProvider),

            };


            app.UseHangfireServer(hangfireOptions);
            app.UseHangfireDashboard();

            return app;
        }
    }

    public class EventGridOptions
    {
        public string BasePath { get; set; } = "Awesome.EventGrid";
        public string TopicsPath { get; set; } = $"topics";
        public string EventsPath { get; set; } = "topics/{topic}/events";
        public string SubscriptionsPath { get; set; } = "subscriptions";
    }
}
