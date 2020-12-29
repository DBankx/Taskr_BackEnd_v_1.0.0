using System;
using AutoMapper;
using Taskr.Domain;
using Taskr.Dtos.Job;

namespace Taskr.MappingProfiles.Job
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Domain.Job, AllJobsDto>().ForMember(x => x.CreatorId, opt => opt.MapFrom(x => x.User.Id))
                .ForMember(x => x.CreatorUsername, opt => opt.MapFrom(x => x.User.UserName));
            CreateMap<ApplicationUser, JobCreatorDto>();
        }
    }
}