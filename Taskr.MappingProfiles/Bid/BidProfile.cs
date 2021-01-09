using AutoMapper;
using Taskr.Dtos;

namespace Taskr.MappingProfiles.Bid
{
    public class BidProfile : Profile
    {
        public BidProfile()
        {
            CreateMap<Domain.Bid, BidDto>();
        }
    }
}