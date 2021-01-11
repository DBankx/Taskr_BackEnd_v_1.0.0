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
            CreateMap<Domain.Job, AllJobsDto>()
                .ForMember(x => x.Creator, opt => opt.MapFrom(x => x.User)).ForMember(x => x.IsWatching, opt => opt.MapFrom<IsWatchingResolverAllJobs>());

            CreateMap<ApplicationUser, JobCreatorDto>();
            CreateMap<Domain.Job, JobDto>().ForMember(x => x.Creator, opt => opt.MapFrom(src => src.User)).ForMember(x => x.BidsCount, opt => opt.MapFrom<BidCountResolver>()).ForMember(x => x.WatchCount, opt => opt.MapFrom<WatchCountResolver>()).ForMember(x => x.IsBidActive, opt => opt.MapFrom<IsBidActiveResolver>()).ForMember(x => x.IsWatching, opt => opt.MapFrom<IsWatchingResolverJobs>());
        }
    }
}