﻿using System;
using AutoMapper;
using Taskr.Domain;
using Taskr.Dtos.Job;

namespace Taskr.MappingProfiles.Job
{
    public class JobProfile : AutoMapper.Profile
    {
        public JobProfile()
        {
            CreateMap<Domain.Job, JobsListDto>()
                .ForMember(x => x.Creator, opt => opt.MapFrom(x => x.User)).ForMember(x => x.IsWatching, opt => opt.MapFrom<IsWatchingResolverAllJobs>());

            CreateMap<ApplicationUser, JobUserDto>();
            CreateMap<Domain.Job, JobDto>().ForMember(x => x.Creator, opt => opt.MapFrom(src => src.User))
                .ForMember(x => x.BidsCount, opt => opt.MapFrom<BidCountResolver>())
                .ForMember(x => x.WatchCount, opt => opt.MapFrom<WatchCountResolver>())
                .ForMember(x => x.IsBidActive, opt => opt.MapFrom<IsBidActiveResolver>())
                .ForMember(x => x.IsWatching, opt => opt.MapFrom<IsWatchingResolverJobs>())
                .ForMember(x => x.IsChatActive, opt => opt.MapFrom<IsChatActiveResolver>())
                .ForMember(x => x.AssignedUser, opt => opt.MapFrom(src => src.AssignedUser))
                .ForMember(x => x.AcceptedBid, opt => opt.MapFrom<AcceptedBidResolver>());
        }
    }
}