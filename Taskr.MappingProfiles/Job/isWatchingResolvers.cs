using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Taskr.Dtos.Job;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.MappingProfiles.Job
{
    public class IsWatchingResolverAllJobs : IValueResolver<Domain.Job, AllJobsDto, bool>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public IsWatchingResolverAllJobs(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        
        public bool Resolve(Domain.Job source, AllJobsDto destination, bool destMember, ResolutionContext context)
        {
            var user = _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId()).Result;

            if (user == null) return false;

            var watching = _context.Watches.SingleOrDefaultAsync(x => x.Job == source && x.User == user).Result;

            return watching != null;

        }
    }
    
    public class IsWatchingResolverJobs : IValueResolver<Domain.Job, JobDto, bool>
        {
            private readonly DataContext _context;
            private readonly IUserAccess _userAccess;
    
            public IsWatchingResolverJobs(DataContext context, IUserAccess userAccess)
            {
                _context = context;
                _userAccess = userAccess;
            }
            
            public bool Resolve(Domain.Job source, JobDto destination, bool destMember, ResolutionContext context)
            {
                var user = _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId()).Result;
    
                if (user == null) return false;
    
                var watching = _context.Watches.SingleOrDefaultAsync(x => x.Job == source && x.User == user).Result;
    
                return watching != null;
    
            }
        }
}