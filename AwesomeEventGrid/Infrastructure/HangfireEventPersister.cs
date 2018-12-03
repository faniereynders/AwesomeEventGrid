using AwesomeEventGrid.Abstractions.Options;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeEventGrid.Infrastructure
{
    public class HangfireEventPersister : IEventPersister
    {
        private readonly string connectionString;
        private readonly bool includeDashboard;
        private readonly BackgroundJobServerOptions hangfireOptions;

        public HangfireEventPersister(string connectionString, BackgroundJobServerOptions hangfireOptions = null, bool includeDashboard = true)
        {
            this.connectionString = connectionString;
            this.includeDashboard = includeDashboard;
            if (hangfireOptions == null)
            {
                this.hangfireOptions = new BackgroundJobServerOptions
                {
                    Queues = new[] { "events" },
                    
                };
            }
            else
            {
                this.hangfireOptions = hangfireOptions;

            }
        }
        public void ConfigureMiddleware(IApplicationBuilder applicationBuilder)
        {
            GlobalJobFilters.Filters.Add(new ManagedStateFilterAttribute(new HangfireActivator(applicationBuilder.ApplicationServices)));
            
            applicationBuilder.UseHangfireServer(hangfireOptions);
            if (includeDashboard)
            {
                applicationBuilder.UseHangfireDashboard();
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
        }
    }
}
