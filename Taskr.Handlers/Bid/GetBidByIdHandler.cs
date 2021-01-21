using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskr.Domain;
using Taskr.Dtos.Errors;
using Taskr.Infrastructure.Helpers;
using Taskr.Persistance;
using Taskr.Queries.Bid;

namespace Taskr.Handlers.Bid
{
    public class GetBidByIdHandler : IRequestHandler<GetBidByIdQuery, Domain.Bid>
    {
        private readonly DataContext _context;
        private readonly IQueryProcessor _queryProcessor;

        public GetBidByIdHandler(DataContext context, IQueryProcessor queryProcessor)
        {
            _context = context;
            _queryProcessor = queryProcessor;
        }
        
        public async Task<Domain.Bid> Handle(GetBidByIdQuery request, CancellationToken cancellationToken)
        {
            var bid = await _queryProcessor.Query<Domain.Bid>()
                .Include(x => x.Job)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            if (bid == null) throw new RestException(HttpStatusCode.NotFound, new {error = "Bid not found"});

            return bid;
        }
    }
}