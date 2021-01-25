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
using Taskr.Persistance;
using Taskr.Queries.Profile;

namespace Taskr.Handlers.Profile
{
    public class GetProfileJobsHandler : IRequestHandler<GetProfileJobsQuery, List<JobsListDto>>
    {
        private readonly DataContext _context;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUserAccess _userAccess;
        private readonly IMapper _mapper;

        public GetProfileJobsHandler(DataContext context, IQueryProcessor queryProcessor, IUserAccess userAccess, IMapper mapper)
        {
            _context = context;
            _queryProcessor = queryProcessor;
            _userAccess = userAccess;
            _mapper = mapper;
        }
        
        public async Task<List<JobsListDto>> Handle(GetProfileJobsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == _userAccess.GetCurrentUserId());

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new {error = "You are unauthorized"});

            List<Job> jobs = await _queryProcessor.Query<Job>()
                        .Include(x => x.Bids)
                        .Include(x => x.Photos)
                        .Include(x => x.User)
                        .Include(x => x.Watching)
                        .ThenInclude(x => x.User)
                        .Where(x => x.User == user && x.JobStatus == request.JobStatus)
                        .ToListAsync(cancellationToken);
                 
            return _mapper.Map<List<JobsListDto>>(jobs);
        }
    }
}