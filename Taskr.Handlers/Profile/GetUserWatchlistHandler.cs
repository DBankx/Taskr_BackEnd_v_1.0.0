using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Dtos.Job;
using Taskr.Infrastructure.Helpers;
using Taskr.Infrastructure.Security;
using Taskr.Queries.Filter;
using Taskr.Queries.Profile;

namespace Taskr.Handlers.Profile
{
    public class GetUserWatchlistHandler : IRequestHandler<GetUserWatchlist, List<JobsListDto>>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUserAccess _userAccess;
        private readonly IMapper _mapper;

        public GetUserWatchlistHandler(IQueryProcessor queryProcessor, IUserAccess userAccess, IMapper mapper)
        {
            _queryProcessor = queryProcessor;
            _userAccess = userAccess;
            _mapper = mapper;
        }
        
        public async Task<List<JobsListDto>> Handle(GetUserWatchlist request, CancellationToken cancellationToken)
        {
            var user = await _queryProcessor.Query<ApplicationUser>().Include(x => x.Watching).ThenInclude(x => x.Job).ThenInclude(x => x.Photos).Include(x => x.Watching).ThenInclude(x => x.Job).ThenInclude(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId(), cancellationToken);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {errors = "You are unauthorized"});

            List<Job> watchedJobs = new List<Job>();

            foreach (var watch in user.Watching)
            {
                watchedJobs.Add(watch.Job);
            }

            var result = AddFilterToQueries.FilterWatchlist(request.SortBy, watchedJobs);
            return _mapper.Map<List<JobsListDto>>(result);
        }
    }
}