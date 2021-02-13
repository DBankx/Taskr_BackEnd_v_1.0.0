using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.Job;
using Taskr.Infrastructure.Helpers;
using Taskr.Queries.PublicProfile;

namespace Taskr.Handlers.PublicProfile
{
    public class GetRecentlyUploadedJobsHandler : IRequestHandler<GetRecentlyUploadedJobs, List<JobsListDto>>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IMapper _mapper;

        public GetRecentlyUploadedJobsHandler(IQueryProcessor queryProcessor, IMapper mapper)
        {
            _queryProcessor = queryProcessor;
            _mapper = mapper;
        }
        
        public async Task<List<JobsListDto>> Handle(GetRecentlyUploadedJobs request, CancellationToken cancellationToken)
        {
            var jobs = await _queryProcessor.Query<Job>().Include(x => x.Photos).Include(x => x.User)
                .Where(x => x.UserId == request.UserId).OrderByDescending(x => x.CreatedAt).Take(6).ToListAsync(cancellationToken);

            return _mapper.Map<List<JobsListDto>>(jobs);
        }
    }
}