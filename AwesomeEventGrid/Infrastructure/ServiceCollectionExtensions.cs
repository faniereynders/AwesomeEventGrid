using AutoMapper;
using AwesomeEventGrid.Stubs;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeEventGrid.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAwesomeEventGrid(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<EventHandler>();
            services.AddSingleton<ITopicsRepository, TopicsRepository>();
            services.AddSingleton<SubscriberDispatcher>();
            services.AddSingleton<ISubscriptionsRepository, SubscriptionsRepository>();
            services.AddSingleton<Data>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
            services.AddAutoMapper();

            return services;
        }

        public static IServiceCollection WithHangFire(this IServiceCollection services, string connectionstring)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(connectionstring));
            return services;
        }
    }
}
