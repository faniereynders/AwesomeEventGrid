using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication23
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

            var url = urlHelper.Link(TopicModel.RouteName, new { name = source.Name });
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

            var url = urlHelper.Link(SubscriptionModel.RouteName, new { name = source.Name });
            return url;
        }
    }
}
