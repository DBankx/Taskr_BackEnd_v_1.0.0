using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Persistance;
using Taskr.Queries.Bid;

namespace Taskr.Handlers.Task
{
    public class GetAllJobsHandler : IRequestHandler<GetAllJobsQuery, IQueryable<Domain.Job>>
    {
        private readonly DataContext _context;

        public GetAllJobsHandler(DataContext context)
        {
            _context = context;
        }
        
        
        public async Task<IQueryable<Domain.Job>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
        {
            return _context.Jobs.AsQueryable();
        }
    }
}