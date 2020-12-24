using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Dtos.ApiResponse;
using Taskr.Dtos.Errors;
using Taskr.Persistance;
using Taskr.Queries.Bid;

namespace Taskr.Handlers.Bid
{
    public class GetBidByIdHandler : IRequestHandler<GetBidByIdQuery, Domain.Bid>
    {
        private readonly DataContext _context;

        public GetBidByIdHandler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<Domain.Bid> Handle(GetBidByIdQuery request, CancellationToken cancellationToken)
        {
            var bid = await _context.Bids.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            if (bid == null) throw new RestException(HttpStatusCode.NotFound, new {error = "Bid not found"});

            return bid;
        }
    }
}