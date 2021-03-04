using Taskr.Domain;
using Taskr.Dtos.Order;

namespace Taskr.MappingProfiles.Order
{
    public class OrderMappingProfile : AutoMapper.Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Domain.Bid, OrderBid>();
            CreateMap<ApplicationUser, OrderUser>();
            CreateMap<Domain.Job, OrderJob>();
            CreateMap<Domain.Order, OrderDetailsDto>()
                .ForMember(x => x.Chat, opt => opt.MapFrom<OrderChatResolver>());
            CreateMap<Domain.Job, OrderListJob>();
            CreateMap<Domain.Order, ListOrderDto>();
        }
    }
}