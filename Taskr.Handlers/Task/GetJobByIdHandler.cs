using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.ApiResponse;
using Taskr.Dtos.Errors;
using Taskr.Persistance;
using Taskr.Queries.Bid;

namespace Taskr.Handlers.Task
{
    public class GetJobByIdHandler : IRequestHandler<GetJobByIdQuery, Domain.Job>
    {
        private readonly DataContext _context;

        public GetJobByIdHandler(DataContext context)
        {
            _context = context;
        }
        
        public async  Task<Domain.Job> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
        {
            var job = await _context.Jobs.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            if (job == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new {Errors = "Job not found"});
            }
            return job;
        }
    }
}