using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Consumer1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration
        {
            get;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddHttpClient();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

       

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationLifetime lifetime, IApplicationBuilder app, IHostingEnvironment env, IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            lifetime.ApplicationStarted.Register(()=>
            {
                var topicUrl = config["TopicUrl"];
                var subscriptions = new List<Subscription>();
                config.GetSection("Subscriptions").Bind(subscriptions);

                var client = httpClientFactory.CreateClient();
                foreach (var subscription in subscriptions)
                {
                    
                    var response = client.PutAsync(topicUrl, subscription, new JsonMediaTypeFormatter()).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                }
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }

    public static class WebHostExtensions
    {
        public static IWebHost SubscribeToEvents(this IWebHost webHost)
        {
            var config = (IConfiguration) webHost.Services.GetService(typeof(IConfiguration));
            var httpClientFactory = (IHttpClientFactory) webHost.Services.GetService(typeof(IHttpClientFactory));

            var topicUrl = config["TopicUrl"];
            var subscriptions = new List<Subscription>();
            config.GetSection("Subscriptions").Bind(subscriptions);

            var client = httpClientFactory.CreateClient();
            foreach (var subscription in subscriptions)
            {
                var response = client.PutAsync(topicUrl, subscription, new JsonMediaTypeFormatter()).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
            }
            

            return webHost;
        }
    }
}
