using Taskr.Domain;
using Taskr.Dtos.Profile;

namespace Taskr.MappingProfiles.Profile
{
    public class ProfileMapping : AutoMapper.Profile
    {
        public ProfileMapping()
        {
            CreateMap<ApplicationUser, ProfileDto>();
            CreateMap<ApplicationUser, PublicProfileDto>()
                .ForMember(x => x.AvgReviewRating, opt => opt.MapFrom<AvgRatingResolver>()).ForMember(x => x.ReviewsCount, opt => opt.MapFrom<ReviewCountResolver>());
        }
    }
}