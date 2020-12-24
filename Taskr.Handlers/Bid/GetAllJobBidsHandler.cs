using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Dtos.ApiResponse;
using Taskr.Persistance;
using Taskr.Queries.Bid;
using Taskr.RepositoryServices.BidService;

namespace Taskr.Handlers.Bid
{
    public class GetAllJobBidsHandler : IRequestHandler<GetAllJobBidsQuery, List<Domain.Bid>>
    {
        private readonly DataContext _context;

        public GetAllJobBidsHandler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<List<Domain.Bid>> Handle(GetAllJobBidsQuery request, CancellationToken cancellationToken)
        {
            
            var bids = await _context.Bids.Where(x => x.JobId == request.JobId).ToListAsync(cancellationToken: cancellationToken);

            return bids;

        }
    }
}