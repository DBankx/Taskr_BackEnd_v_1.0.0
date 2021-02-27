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
            CreateMap<Domain.Order, OrderDetailsDto>();
        }
    }
}