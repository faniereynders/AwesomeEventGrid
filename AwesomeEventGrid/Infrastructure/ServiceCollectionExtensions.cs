using AutoMapper;
using AwesomeEventGrid.Stubs;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AwesomeEventGrid.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAwesomeEventGrid(this IServiceCollection services, Action<EventGridOptions> options = null)
        {
            if (options == null)
            {
                options = (a) => new EventGridOptions();
            }
            services.AddHttpClient();
            services.AddSingleton<DefaultEventGridEventHandler>();
            services.AddSingleton<ITopicsRepository, TopicsRepository>();
            services.AddSingleton<SubscriberDispatcher>();
            services.AddSingleton<ISubscriptionsRepository, SubscriptionsRepository>();
            services.AddSingleton<Data>();

            //services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            //services.AddScoped<IUrlHelper>(x =>
            //{
            //    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
            //    var factory = x.GetRequiredService<IUrlHelperFactory>();
            //    return factory.GetUrlHelper(actionContext);
            //});
            services.AddAutoMapper();

            services.AddOptions<EventGridOptions>();
            services.Configure(options);
            return services;
        }

        public static IServiceCollection WithHangFire(this IServiceCollection services, string connectionstring)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(connectionstring));
            return services;
        }
    }
}
