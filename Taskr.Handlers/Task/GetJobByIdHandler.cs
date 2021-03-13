using System.Collections.Generic;
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
using Taskr.Persistance;
using Taskr.Queries.Bid;

namespace Taskr.Handlers.Task
{
    public class GetJobByIdHandler : IRequestHandler<GetJobByIdQuery, JobDto>
    {
        private readonly IMapper _mapper;
        private readonly IQueryProcessor _queryProcessor;

        public GetJobByIdHandler( IMapper mapper, IQueryProcessor queryProcessor)
        {
            _mapper = mapper;
            _queryProcessor = queryProcessor;
        }
        
        public async  Task<JobDto> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
        {
            var job = await _queryProcessor.Query<Job>()
                .Include(x => x.User)
                .Include(x => x.Photos)
                .Include(x => x.Bids)
                .Include(x => x.Watching)
                .Include(x => x.AssignedUser)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            
            if (job == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new {Errors = "Job not found"});
            }
            
            return _mapper.Map<JobDto>(job);
        }
    }
}