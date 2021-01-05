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
using Taskr.Persistance;
using Taskr.Queries.Bid;

namespace Taskr.Handlers.Task
{
    public class GetJobByIdHandler : IRequestHandler<GetJobByIdQuery, JobDto>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public GetJobByIdHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async  Task<JobDto> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
        {
            var job = await _context.Jobs.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            if (job == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new {Errors = "Job not found"});
            }

            job.Views++;
            
            return _mapper.Map<JobDto>(job);
        }
    }
}