using Taskr.Domain;
using Taskr.Dtos.Review;

namespace Taskr.MappingProfiles.Review
{
    public class ReviewMappingProfile : AutoMapper.Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<ApplicationUser, ReviewUserDto>();
            CreateMap<Domain.Job, ReviewJobDto>();
            CreateMap<Domain.Review, ReviewDto>()
                .ForMember(x => x.Job, opt => opt.MapFrom(x => x.Order.Job))
                .ForMember(x => x.Reviewer, opt => opt.MapFrom(x => x.Reviewer));
        }   
    }
}