using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using WebApplication23.Controllers;

namespace WebApplication23
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<EventHandler>();
            services.AddSingleton<ITopicsRepository,TopicsRepository>();
            services.AddSingleton<SubscriberDispatcher>();
            services.AddSingleton<ISubscriptionsRepository, SubscriptionsRepository>();
            services.AddSingleton<Data>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
            services.AddAutoMapper();
            //Mapper.Initialize(cfg =>
            //{
            //    var container = services.BuildServiceProvider();
            //    foreach (var configuration in container.GetServices<IAutoMapperConfiguration>())
            //    {
            //        configuration.Configure(cfg);
            //    }
            //});
            //services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.InvalidModelStateResponseFactory = context =>
            //    {
            //        if (context.ModelState.ValidationState.)
            //        {

            //        }
            //        var problemDetails = new ValidationProblemDetails(context.ModelState)
            //        {
            //            Instance = context.HttpContext.Request.Path,
            //            Status = StatusCodes.Status400BadRequest,
            //        };
            //        return new BadRequestObjectResult(problemDetails)
            //        {
            //            ContentTypes = { "application/problem+json", "application/problem+xml" }
            //        };
            //    };
            //});




            services.AddHangfire(x => 
                x.UseSqlServerStorage("Server=(localdb)\\mssqllocaldb;Database=EventGrid;Trusted_Connection=True;"));
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void OnApplicationStarted()
        {
            // Carry out your initialisation.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            
            //GlobalConfiguration.Configuration.UseActivator(new ServiceProviderJobActivator(app.ApplicationServices));
            // GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(serviceProvider));
            GlobalJobFilters.Filters.Add(new ManagedStateFilterAttribute(new HangfireActivator(app.ApplicationServices)));
            var hangfireOptions = new BackgroundJobServerOptions
            {
                Queues = new[] { "events" },
              //  Activator = new HangfireActivator(serviceProvider),
                
            };

            
            app.UseHangfireServer(hangfireOptions);
            app.UseHangfireDashboard();

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
    public class HangfireActivator : Hangfire.JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public HangfireActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type type)
        {
            return _serviceProvider.GetService(type);
        }
    }
}
