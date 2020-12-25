using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Persistance;
using Taskr.Queries.Bid;

namespace Taskr.Handlers.Bid
{
    public class GetAllJobBidsHandler : IRequestHandler<GetAllJobBidsQuery, IQueryable<Domain.Bid>>
    {
        private readonly DataContext _context;

        public GetAllJobBidsHandler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<IQueryable<Domain.Bid>> Handle(GetAllJobBidsQuery request, CancellationToken cancellationToken)
        {
            
            var bids = _context.Bids.Where(x => x.JobId == request.JobId).AsQueryable();

            return bids;

        }
    }
}