using Hangfire;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeEventGrid.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAwesomeEventGrid(this IApplicationBuilder app)
        {
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
}
