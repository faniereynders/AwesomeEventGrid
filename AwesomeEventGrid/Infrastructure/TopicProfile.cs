using AutoMapper;
using AwesomeEventGrid.Entities;
using AwesomeEventGrid.Models;
using Microsoft.Extensions.Options;

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

        private readonly IOptions<EventGridOptions> options;

        public TopicIdValueResolver(IOptions<EventGridOptions> options)
        {
            this.options = options;
        }
        public string Resolve(Topic source, TopicModel destination, string destMember, ResolutionContext context)
        {

            var url = $"{options.Value.BasePath}/{options.Value.TopicsPath}/{source.Name }";
            return url;
        }
    }

    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<Subscription, SubscriptionModel>()
               // .ForMember(t => t.Id, o => o.MapFrom<SubscriptionIdValueResolver>())
                .ReverseMap();
        }
    }

    //public class SubscriptionIdValueResolver : IValueResolver<Subscription, SubscriptionModel, string>
    //{
    //    private readonly IUrlHelper urlHelper;

    //    public SubscriptionIdValueResolver(IUrlHelper urlHelper)
    //    {
    //        this.urlHelper = urlHelper;
    //    }
    //    public string Resolve(Subscription source, SubscriptionModel destination, string destMember, ResolutionContext context)
    //    {

    //        var url = urlHelper.Link(Constants.Subscriptions.RouteName, new { name = source.Name });
    //        return url;
    //    }
    //}
}
