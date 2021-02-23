using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.Job;
using Taskr.Infrastructure.Helpers;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.MappingProfiles.Job
{
    public class IsWatchingResolverAllJobs : IValueResolver<Domain.Job, JobsListDto, bool>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUserAccess _userAccess;

        public IsWatchingResolverAllJobs(IQueryProcessor queryProcessor, IUserAccess userAccess)
        {
            _queryProcessor = queryProcessor;
            _userAccess = userAccess;
        }
        
        public bool Resolve(Domain.Job source, JobsListDto destination, bool destMember, ResolutionContext context)
        {
            var user = _queryProcessor.Query<ApplicationUser>().SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId()).Result;

            if (user == null) return false;

            var watching = _queryProcessor.Query<Watch>().SingleOrDefaultAsync(x => x.Job == source && x.User == user).Result;

            return watching != null;

        }
    }
    
    public class IsWatchingResolverJobs : IValueResolver<Domain.Job, JobDto, bool>
        {
            private readonly IQueryProcessor _queryProcessor;
            private readonly IUserAccess _userAccess;

            public IsWatchingResolverJobs(IQueryProcessor queryProcessor, IUserAccess userAccess)
            {
                _queryProcessor = queryProcessor;
                _userAccess = userAccess;
            }
            
            public bool Resolve(Domain.Job source, JobDto destination, bool destMember, ResolutionContext context)
            {
                var user = _queryProcessor.Query<ApplicationUser>().SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId()).Result;
    
                if (user == null) return false;
    
                var watching = _queryProcessor.Query<Watch>().SingleOrDefaultAsync(x => x.Job == source && x.User == user).Result;
    
                return watching != null;
    
            }
        }
}