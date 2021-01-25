using AutoMapper;
using Taskr.Dtos;
using Taskr.Dtos.Bid;

namespace Taskr.MappingProfiles.Bid
{
    public class BidProfile : AutoMapper.Profile
    {
        public BidProfile()
        {
            CreateMap<Domain.Bid, BidDto>();
            CreateMap<Domain.Bid, TaskBidDto>()
            .ForMember(x => x.Avatar, opt => opt.MapFrom(src => src.User.Avatar))
            .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.User.UserName));
        }
    }
}