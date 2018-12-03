using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AwesomeEventGrid.Endpoints;
using Microsoft.AspNetCore.Routing;
using AwesomeEventGrid.Abstractions.Options;

namespace AwesomeEventGrid.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAwesomeEventGrid(this IApplicationBuilder app)
        {
            

            var options = app.ApplicationServices.GetRequiredService<IOptions<AwesomeEventGridOptions>>();
            var eventPersister = app.ApplicationServices.GetRequiredService<IEventPersister>();
            // config.Bind(options);

            var routeHandler = new RouteHandler(context =>
            {
               
                return context.Response.WriteAsync(
                    $"Hello!");
            });

            var routeBuilder = new RouteBuilder(app, routeHandler);

            var topicsPath = $"{options.Value.BasePath}/{options.Value.TopicsPath}";

            routeBuilder.MapMiddlewareGet(topicsPath, b => b.UseMiddleware<TopicsGetAllEndpoint>());
            routeBuilder.MapMiddlewarePost(topicsPath, b => b.UseMiddleware<TopicsCreateEndpoint>());
            routeBuilder.MapMiddlewareGet(topicsPath + "/{name}", b => b.UseMiddleware<TopicsGetByNameEndpoint>());
            routeBuilder.MapMiddlewarePost(topicsPath + "/{name}", b => b.UseMiddleware<TopicsEventsEndpoint>());


            var subscriptionsPath = $"{options.Value.BasePath}/{options.Value.SubscriptionsPath}";

            routeBuilder.MapMiddlewareGet(subscriptionsPath, b => b.UseMiddleware<SubscriptionsGetAllEndpoint>());
            routeBuilder.MapMiddlewarePut(subscriptionsPath + "/{topic}", b => b.UseMiddleware<SubscriptionsCreateOrUpdateEndpoint>());
            routeBuilder.MapMiddlewareGet(subscriptionsPath + "/{topic}/{name}", b => b.UseMiddleware<SubscriptionsGetByNameEndpoint>());


            


            var routes = routeBuilder.Build();
            app.UseRouter(routes);


            eventPersister.ConfigureMiddleware(app);
            //GlobalJobFilters.Filters.Add(new ManagedStateFilterAttribute(new HangfireActivator(app.ApplicationServices)));
            //var hangfireOptions = new BackgroundJobServerOptions
            //{
            //    Queues = new[] { "events" },
            //    //  Activator = new HangfireActivator(serviceProvider),
                
            //};


            //app.UseHangfireServer(hangfireOptions);
            //app.UseHangfireDashboard();

            return app;
        }
    }
}
