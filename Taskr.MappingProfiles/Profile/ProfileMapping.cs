using Taskr.Domain;
using Taskr.Dtos.Profile;

namespace Taskr.MappingProfiles.Profile
{
    public class ProfileMapping : AutoMapper.Profile
    {
        public ProfileMapping()
        {
            CreateMap<ApplicationUser, ProfileDto>();
        }
    }
}