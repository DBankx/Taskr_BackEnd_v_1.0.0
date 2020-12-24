using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Persistance;
using Taskr.Queries.Bid;

namespace Taskr.Handlers.Task
{
    public class GetAllJobsHandler : IRequestHandler<GetAllJobsQuery, List<Domain.Job>>
    {
        private readonly DataContext _context;

        public GetAllJobsHandler(DataContext context)
        {
            _context = context;
        }
        
        
        public async Task<List<Domain.Job>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Jobs.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}