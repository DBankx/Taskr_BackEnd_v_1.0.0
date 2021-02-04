using AutoMapper;
using Taskr.Domain;
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
            CreateMap<ApplicationUser, BidCreatorDto>();
            CreateMap<Domain.Bid, SingleBidDto>().ForMember(x => x.BidCreator, opt => opt.MapFrom(s => s.User));
        }
    }
}