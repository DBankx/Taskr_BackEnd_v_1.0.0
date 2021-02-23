using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.Job;
using Taskr.Infrastructure.Helpers;
using Taskr.Infrastructure.Security;

namespace Taskr.MappingProfiles.Job
{
    public class IsChatActiveResolver : IValueResolver<Domain.Job, JobDto, bool>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUserAccess _userAccess;

        public IsChatActiveResolver(IQueryProcessor queryProcessor, IUserAccess userAccess)
        {
            _queryProcessor = queryProcessor;
            _userAccess = userAccess;
        }
        
        public bool Resolve(Domain.Job source, JobDto destination, bool destMember, ResolutionContext context)
        {
            var loggedInUser = _queryProcessor.Query<ApplicationUser>()
                .SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId()).Result;

            if (loggedInUser == null) return false;

            var chat = _queryProcessor.Query<Domain.Chat>()
                .SingleOrDefaultAsync(x => x.Job == source && x.Runner == loggedInUser).Result;

            if (chat != null) return true;

            return false;
        }
    }
}