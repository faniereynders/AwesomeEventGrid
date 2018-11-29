using AutoMapper;
using AwesomeEventGrid.Entities;
using AwesomeEventGrid.Infrastructure;
using AwesomeEventGrid.Models;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeEventGrid.Infrastructure
{


    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
            CreateMap<Topic, TopicModel>()
                .ForMember(t => t.Id, o => o.MapFrom<TopicIdValueResolver>())
                .ReverseMap();
        }
    }

    public class TopicIdValueResolver : IValueResolver<Topic, TopicModel, string>
    {
        private readonly IUrlHelper urlHelper;

        public TopicIdValueResolver(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }
        public string Resolve(Topic source, TopicModel destination, string destMember, ResolutionContext context)
        {

            var url = urlHelper.Link(Constants.Topics.RouteName, new { name = source.Name });
            return url;
        }
    }

    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<Subscription, SubscriptionModel>()
                .ForMember(t => t.Id, o => o.MapFrom<SubscriptionIdValueResolver>())
                .ReverseMap();
        }
    }

    public class SubscriptionIdValueResolver : IValueResolver<Subscription, SubscriptionModel, string>
    {
        private readonly IUrlHelper urlHelper;

        public SubscriptionIdValueResolver(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }
        public string Resolve(Subscription source, SubscriptionModel destination, string destMember, ResolutionContext context)
        {

            var url = urlHelper.Link(Constants.Subscriptions.RouteName, new { name = source.Name });
            return url;
        }
    }
}
