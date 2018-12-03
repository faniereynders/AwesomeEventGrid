using AutoMapper;
using AwesomeEventGrid.Abstractions;
using AwesomeEventGrid.Abstractions.Options;
using AwesomeEventGrid.Stubs;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace AwesomeEventGrid.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAwesomeEventGrid(this IServiceCollection services, IEventPersister eventPersister, Action<AwesomeEventGridOptions> options = null)
        {
            if (options == null)
            {
                options = o => new AwesomeEventGridOptions();
            }
            services.AddHttpClient();
            services.TryAddScoped<IEventGridEventHandler, DefaultEventGridEventHandler>();
            services.TryAddSingleton<ITopicsRepository, TopicsRepository>();
            services.TryAddSingleton<ISubscriberEventDispatcher,HangfireSubscriberEventDispatcher>();
            services.TryAddSingleton<ISubscriptionsRepository, SubscriptionsRepository>();
            services.TryAddSingleton(eventPersister);
            services.AddSingleton<Data>();

            services.AddAutoMapper();

            services.AddOptions<AwesomeEventGridOptions>();
            services.Configure(options);

            
            eventPersister.ConfigureServices(services);
            

            return services;
        }

        //public static IServiceCollection WithHangFire(this IServiceCollection services, string connectionstring)
        //{
        //    services.AddHangfire(x => x.UseSqlServerStorage(connectionstring));
        //    return services;
        //}
    }
}
