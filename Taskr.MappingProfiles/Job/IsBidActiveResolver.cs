using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Taskr.Dtos.Job;
using Taskr.Infrastructure.Security;
using Taskr.Persistance;

namespace Taskr.MappingProfiles.Job
{
    public class IsBidActiveResolver : IValueResolver<Domain.Job, JobDto, bool>
    {
        private readonly DataContext _context;
        private readonly IUserAccess _userAccess;

        public IsBidActiveResolver(DataContext context, IUserAccess userAccess)
        {
            _context = context;
            _userAccess = userAccess;
        }
        
        public bool Resolve(Domain.Job source, JobDto destination, bool destMember, ResolutionContext context)
        {
            var user = _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId()).Result;

            if (user == null)
            {
                return false;
            }

            var bid = _context.Bids.SingleOrDefaultAsync(x => x.JobId == source.Id && x.UserId == user.Id).Result;

            return bid != null;
        }
    }
}